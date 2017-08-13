using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Handlers;
using Core.Domain.Model.Suppliers;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for SupplierDuePurchasesWindow.xaml
    /// </summary>
    public partial class SupplierDuePurchasesWindow
    {
        private readonly PurchaseHandler _purchaseHandler;

        public SupplierDuePurchasesWindow(Supplier supplier)
        {
            _purchaseHandler = new PurchaseHandler();
            var supplierHandler = new SupplierHandler();
            if (string.IsNullOrEmpty(supplier.Name))
                supplier = supplierHandler.GetSuppliers(supplier.Id).SingleOrDefault();
            if (supplier == null) throw new ArgumentNullException(nameof(supplier), @"Supplier Id is null");
            DuePurchases = _purchaseHandler.GetPurchases(supplier.Id, false);
            InitializeComponent();
            SupplierIdTextBox.Text = supplier.Id.ToString();
            NameTextBox.Text = supplier.Name;
        }

        public IEnumerable<Purchase> DuePurchases { get; private set; }

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
            _purchaseHandler.UpdatePurchaseMultiple(PurchasesDataGrid.SelectedItems.Cast<Purchase>(), true);
            MessageBox.Show("Successfully Updated!!", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }

        private void SupplierDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AmountTextBox.Text =
                PurchasesDataGrid.SelectedItems.Cast<Purchase>().Sum(order => order.Amount)
                    .ToString(CultureInfo.InvariantCulture);
        }
    }
}