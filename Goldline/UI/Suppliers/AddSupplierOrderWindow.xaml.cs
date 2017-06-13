using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Model.Enums;
using Core.Model.Handlers;
using Core.Model.Orders;
using Core.Model.Persons;
using Core.Model.Products;
using Goldline.UI.Invoices;
using log4net;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for AddSupplierOrderWindow.xaml
    /// </summary>
    public partial class AddSupplierOrderWindow : Window
    {
        private static readonly ILog Logger = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);

        private readonly OrderHandler _orderHandler;
        private readonly ProductHandler _productHandler;

        public AddSupplierOrderWindow()
        {
            // Set up variables and properties prior to initialization
            _orderHandler = new OrderHandler();
            _productHandler = new ProductHandler();
            SupplierOrder = new SupplierOrder();
            SupplierSource = new SupplierHandler().GetAllSuppliers();
            ItemSource = _productHandler.GetItems("");

            InitializeComponent();
            Logger.Info("AddSupplierOrderWindow Loaded successfully");
        }

        public IEnumerable<Supplier> SupplierSource { get; set; }
        public IEnumerable<Item> ItemSource { get; set; }
        public SupplierOrder SupplierOrder { get; set; }

        /// <summary>
        ///     Reset Qty and Price textbox layout after adding an order entry or an express checkout
        /// </summary>
        private void InitializeTextBoxes()
        {
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
        }

        /// <summary>
        ///     Reset window layout after completing and order and get ready for adding new order
        /// </summary>
        public void InitializeNewSupplyOrder()
        {
            SupplierOrder = new SupplierOrder();
            InitializeTextBoxes();
            TotalAmountTextBox.Text = "";
            NoteTextBox.Text = "";
            CheckBox.IsChecked = false;
            RefreshInventoryDataGrid();
            RefreshSupplyOrderEntriesDataGrid();
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
            if (InventoryDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please Select a item", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            if (QuantityTextBox.Text == "" || PriceTextBox.Text == "")
            {
                MessageBox.Show("Please Enter Quantity and Price", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return false;
            }
            if (QuantityTextBox.Text == "0" || PriceTextBox.Text == "0")
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
            //try
            //{
            // Set total and price variables before adding
            _orderHandler.AddSupplyOrder(SupplierOrder);
            MessageBox.Show("Successfully Added", "Information", MessageBoxButton.OK,
                MessageBoxImage.Information);
            InitializeNewSupplyOrder();
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Not Added", "Error", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //}
        }

        public void AddSelectedItemToOrder()
        {
            var orderEntry = new SupplierOrderEntry((Item) InventoryDataGrid.SelectedItem,
                uint.Parse(QuantityTextBox.Text), decimal.Parse(PriceTextBox.Text));
            SupplierOrder.AddOrderEntry(orderEntry);
            RefreshSupplyOrderEntriesDataGrid();
        }

        #region DataGrid Refresh Methods

        private void RefreshInventoryDataGrid()
        {
            ItemSource = _productHandler.GetItems(SearchTextBox.Text);
            InventoryDataGrid.Items.Refresh();
        }

        private void RefreshSupplyOrderEntriesDataGrid()
        {
            SupplyOrderEntriesDataGrid.Items.Refresh();
        }

        #endregion

        #region Event Handling

        private void SupplierComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = ((Supplier) SupplierComboBox.SelectedItem).Id;
            if (id != null)
                SupplierOrder.SupplierId = id.Value;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshInventoryDataGrid();
        }

        private void SupplyOrderWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (InventoryDataGrid.SelectedIndex < InventoryDataGrid.Items.Count - 1)
                        InventoryDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (InventoryDataGrid.SelectedIndex > 0) InventoryDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        #endregion

        #region ButtonClick Events

        private void AddToOrderButton_Click(object sender, RoutedEventArgs e)
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
            SupplierOrder.Note = NoteTextBox.Text;
            if (!SupplierOrder.OrderEntries.Any())
            {
                MessageBox.Show("No entries found in order!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            else
            {
                var confirmationWindow = new SupplierOrderInvoice(SupplierOrder,
                    (Supplier) SupplierComboBox.SelectedItem);
                confirmationWindow.ShowDialog();
                if (!confirmationWindow.IsVerified) return;
                CompleteOrder();
            }
        }

        private void RemoveEntryButton_Click(object sender, RoutedEventArgs e)
        {
            SupplierOrder.RemoveOrderEntry((SupplierOrderEntry) SupplyOrderEntriesDataGrid.SelectedItem);
            RefreshSupplyOrderEntriesDataGrid();
        }

        private void CheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            SupplierOrder.Status = CheckBox.IsChecked == true ? SupplyOrderStatus.Paid : SupplyOrderStatus.Pending;
        }

        #endregion
    }
}