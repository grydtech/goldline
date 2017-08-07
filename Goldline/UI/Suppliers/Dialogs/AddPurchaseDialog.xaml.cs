using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;
using Core.Domain.Model.Suppliers;
using Goldline.UI.Invoices;
using log4net;

namespace Goldline.UI.Suppliers.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddPurchaseDialog.xaml
    /// </summary>
    public partial class AddPurchaseDialog : Window
    {
        private static readonly ILog Logger = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ProductHandler _productHandler;
        private readonly PurchaseHandler _purchaseHandler;

        public AddPurchaseDialog()
        {
            // Set up variables and properties prior to initialization
            _purchaseHandler = new PurchaseHandler();
            _productHandler = new ProductHandler();
            Purchase = new Purchase();
            SupplierSource = new SupplierHandler().GetSuppliers();
            ItemSource = _productHandler.GetItems();

            InitializeComponent();
            Logger.Info("AddPurchaseDialog Loaded successfully");
        }

        public IEnumerable<Supplier> SupplierSource { get; set; }
        public IEnumerable<Item> ItemSource { get; set; }
        public Purchase Purchase { get; set; }

        /// <summary>
        ///     Reset Qty and Price textbox layout after adding an order entry or an express checkout
        /// </summary>
        private void InitializeTextBoxes()
        {
            QuantityTextBox.Text = "";
        }

        /// <summary>
        ///     Reset window layout after completing and order and get ready for adding new order
        /// </summary>
        public void InitializeNewSupplyOrder()
        {
            Purchase = new Purchase();
            InitializeTextBoxes();
            TotalAmountTextBox.Text = "";
            NoteTextBox.Text = "";
            RefreshSearchProductComboBox();
            RefreshPurchaseEntriesDataGrid();
        }

        /// <summary>
        ///     Checkout currently selected Item with typed quantity and price.
        ///     Useful when only one type of item is bought
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpressCheckoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddSelectedItemToOrder();
            CompleteOrder();
            MessageBox.Show("Successfully Checked Out", "Information", MessageBoxButton.OK,
                MessageBoxImage.Information);

            // Initialize for a new supply order
            InitializeNewSupplyOrder();
        }

        #region Validation

        private bool IsItemEligibleForEntry()
        {
            if (SearchProductComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please Select a item", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            if (QuantityTextBox.Text == "")
            {
                MessageBox.Show("Please Enter Quantity and Price", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return false;
            }
            if (QuantityTextBox.Text == "0")
            {
                MessageBox.Show("Quantity and Price must be greater than 0", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        #endregion

        public void CompleteOrder()
        {
            _purchaseHandler.AddPurchase(Purchase);
            MessageBox.Show("Successfully Added", "Information", MessageBoxButton.OK,
                MessageBoxImage.Information);
            InitializeNewSupplyOrder();
        }

        public void AddSelectedItemToOrder()
        {
            var item = (Item) SearchProductComboBox.SelectedItem;
            if (item.Id == null) return;
            var purchaseItem = new PurchaseItem(item.Id.Value, item.Name, uint.Parse(QuantityTextBox.Text));
            Purchase.AddPurchaseItem(purchaseItem);
            RefreshPurchaseEntriesDataGrid();
        }

        #region DataGrid Refresh Methods

        private void RefreshSearchProductComboBox()
        {
            ItemSource = _productHandler.GetItems();
            SearchProductComboBox.GetBindingExpression(ProductComboBox.ItemsSourceProperty)?.UpdateTarget();
        }

        private void RefreshPurchaseEntriesDataGrid()
        {
            PurchaseEntriesDataGrid.Items.Refresh();
        }

        #endregion

        #region Event Handling

        private void SupplierComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = ((Supplier) SupplierComboBox.SelectedItem).Id;
            if (id != null)
                Purchase.SupplierId = id.Value;
        }

        #endregion

        #region ButtonClick Events

        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsItemEligibleForEntry()) AddSelectedItemToOrder();
            InitializeTextBoxes();
            QuantityTextBox.Focus();
        }

        private void CancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeNewSupplyOrder();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            Purchase.Note = NoteTextBox.Text;
            if (!Purchase.PurchaseItems.Any())
            {
                MessageBox.Show("No entries found in order!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            else
            {
                CompleteOrder();
            }
        }

        private void RemoveEntryButton_Click(object sender, RoutedEventArgs e)
        {
            Purchase.RemovePurchaseItem((PurchaseItem) PurchaseEntriesDataGrid.SelectedItem);
            RefreshPurchaseEntriesDataGrid();
        }

        #endregion
    }
}