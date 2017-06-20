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
        private readonly OrderHandler _orderHandler;
        private readonly Supplier _supplier;

        public SupplierPaymentsWindow(Supplier supplier)
        {
            _supplier = supplier;
            _orderHandler = new OrderHandler();
            DueSupplyOrders = _orderHandler.GetDueSupplyOrders(_supplier);

            InitializeComponent();
            SupplierIdTextBox.Text = _supplier.Id.ToString();
            NameTextBox.Text = _supplier.Name;
        }

        public IEnumerable<Purchase> DueSupplyOrders { get; private set; }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            if (SupplyOrdersDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please Select supply orders", "Nothing Selected", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var messageBoxResult = MessageBox.Show("Confirm payment", "Confirm", MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Cancel) return;

            // payoff all selected supplier orders
            _orderHandler.PayoffSupplyOrders((IEnumerable<Purchase>) SupplyOrdersDataGrid.SelectedItems);
            MessageBox.Show("Successfully Updated!!", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshDataGrid();
        }

        private void SupplierDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AmountTextBox.Text =
                SupplyOrdersDataGrid.SelectedItems.Cast<Purchase>().Sum(order => order.Amount).ToString();
        }

        private void RefreshDataGrid()
        {
            DueSupplyOrders = _orderHandler.GetDueSupplyOrders(_supplier);
            SupplyOrdersDataGrid.Items.Refresh();
        }
    }
}