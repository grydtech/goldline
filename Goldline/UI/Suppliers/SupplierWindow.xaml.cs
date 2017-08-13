using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Suppliers;
using Goldline.UI.Suppliers.Dialogs;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for SupplierWindow.xaml
    /// </summary>
    public partial class SupplierWindow : Window
    {
        private readonly SupplierHandler _supplierHandler;

        public SupplierWindow()
        {
            _supplierHandler = new SupplierHandler();
            SupplierSource = _supplierHandler.GetSuppliers();
            InitializeComponent();
        }

        public IEnumerable<Supplier> SupplierSource { get; private set; }

        private bool IsAllDataEntered()
        {
            var result = NameTextBox.Text != "" && ContactInfoTextBox.Text != "";
            if (result == false)
                MessageBox.Show("Please enter all data correctly to proceed");
            return result;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (SupplierDataGrid.SelectedIndex < SupplierDataGrid.Items.Count - 1)
                        SupplierDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (SupplierDataGrid.SelectedIndex > 0) SupplierDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        #region ProductComboBox

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        #endregion

        #region Data Grids

        private void RefreshDataGrid()
        {
            SupplierSource = _supplierHandler.GetSuppliers(name: SearchTextBox.Text);
            SupplierDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            SupplierDataGrid?.Items.Refresh();
        }

        #endregion

        #region Buttons

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            new AddSupplierDialog().ShowDialog();
            RefreshDataGrid();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAllDataEntered()) return;
            _supplierHandler.UpdateSupplierDetails((Supplier) SupplierDataGrid.SelectedItem, NameTextBox.Text,
                ContactInfoTextBox.Text);
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void BtnSupplierPayments_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var supplier = button?.Tag as Supplier;
            if (supplier == null)
                MessageBox.Show("Please select a customer", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            else
                new SupplierDuePurchasesWindow(supplier).ShowDialog();
            RefreshDataGrid();
        }

        #endregion
    }
}