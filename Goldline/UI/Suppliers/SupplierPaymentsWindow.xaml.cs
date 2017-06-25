using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Handlers;
using Core.Domain.Model.Suppliers;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for SupplierPaymentsWindow.xaml
    /// </summary>
    public partial class SupplierPaymentsWindow : Window
    {
        private readonly PurchaseHandler _purchaseHandler;
        private readonly Supplier _supplier;

        public SupplierPaymentsWindow(Supplier supplier)
        {
            _supplier = supplier;
            _purchaseHandler = new PurchaseHandler();
            DueSupplyOrders = _purchaseHandler.GetPurchases(_supplier.Id);
            InitializeComponent();
            SupplierIdTextBox.Text = _supplier.Id.ToString();
            NameTextBox.Text = _supplier.Name;
        }

        public IEnumerable<Purchase> DueSupplyOrders { get; private set; }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            if (PurchasesDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please Select supply orders", "Nothing Selected", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var messageBoxResult = MessageBox.Show("Confirm payment", "Confirm", MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Cancel) return;

            // payoff all selected supplier orders
            _purchaseHandler.UpdatePurchaseMultiple((IEnumerable<Purchase>) PurchasesDataGrid.SelectedItems, true);
            MessageBox.Show("Successfully Updated!!", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshDataGrid();
        }

        private void SupplierDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AmountTextBox.Text =
                PurchasesDataGrid.SelectedItems.Cast<Purchase>().Sum(order => order.Amount).ToString();
        }

        private void RefreshDataGrid()
        {
            DueSupplyOrders = _purchaseHandler.GetPurchases(_supplier.Id, false);
            PurchasesDataGrid.Items.Refresh();
        }
    }
}