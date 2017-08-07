using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;
using Core.Domain.Model.Suppliers;
using log4net;

namespace Goldline.UI.Suppliers.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddPurchaseDialog.xaml
    /// </summary>
    public partial class AddPurchaseDialog
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        ///     Reset window layout after completing and Purchase and get ready for adding new Purchase
        /// </summary>
        public void InitializeNewPurchase()
        {
            Purchase = new Purchase();
            TextBoxQty.Clear();
            TotalAmountTextBox.Clear();
            NoteTextBox.Clear();
            RefreshSearchProductComboBox();
            RefreshPurchaseEntriesDataGrid();
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
            if (TextBoxQty.Text == "")
            {
                MessageBox.Show("Please Enter Quantity and Price", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return false;
            }
            if (TextBoxQty.Text == "0")
            {
                MessageBox.Show("Quantity and Price must be greater than 0", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        #endregion

        public void CompletePurchase()
        {
            _purchaseHandler.AddPurchase(Purchase);
            MessageBox.Show("Successfully Added", "Information", MessageBoxButton.OK,
                MessageBoxImage.Information);
            InitializeNewPurchase();
        }

        public void AddSelectedItemToPurchase()
        {
            var item = (Item) SearchProductComboBox.SelectedItem;
            if (item.Id == null) return;
            var purchaseItem = new PurchaseItem(item.Id.Value, item.Name, uint.Parse(TextBoxQty.Text));
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

        #region ButtonClick Events

        private void ButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsItemEligibleForEntry()) AddSelectedItemToPurchase();
            TextBoxQty.Clear();
            TextBoxQty.Focus();
            SearchProductComboBox.Text = string.Empty;
        }


        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            Purchase.Note = NoteTextBox.Text;
            if (!Purchase.PurchaseItems.Any())
                MessageBox.Show("No entries found in Purchase!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            else
                CompletePurchase();
        }

        private void RemoveEntryButton_Click(object sender, RoutedEventArgs e)
        {
            var purchaseItem = (sender as Button)?.Tag as PurchaseItem;
            if (purchaseItem == null) return;
            Purchase.PurchaseItems.Remove(purchaseItem);
            RefreshPurchaseEntriesDataGrid();
        }

        #endregion
    }
}