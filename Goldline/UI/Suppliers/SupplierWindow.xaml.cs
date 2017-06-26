using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Suppliers;

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
            SupplierSource = _supplierHandler.GetSuppliers("");
            InitializeComponent();
            RefreshListBox();
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

        #region SearchTextBox

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        #endregion

        #region Data Grids

        private void RefreshDataGrid()
        {
            SupplierSource = _supplierHandler.GetSuppliers(SearchTextBox.Text);
            SupplierDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            SupplierDataGrid?.Items.Refresh();
        }

        private void RefreshListBox()
        {
            ListBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            ListBox?.Items.Refresh();
        }

        #endregion

        #region Buttons

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.AddSupplierDialog().ShowDialog();
            RefreshDataGrid();
            RefreshListBox();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAllDataEntered()) return;

            foreach (var supplier in SupplierSource)
                _supplierHandler.UpdateSupplierDetails(supplier);
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void PaySupplierButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SupplierDataGrid.SelectedItem == null)
                MessageBox.Show("Please select a customer", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            else
                new PurchasePaymentWindow((Supplier) SupplierDataGrid.SelectedItem).ShowDialog();
            RefreshDataGrid();
            RefreshListBox();
        }

        private void ItemAddButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSupplier = (Supplier) SupplierDataGrid.SelectedItem;
            if (selectedSupplier == null) return;
            var addSuppliedItemWindow = new Dialogs.AddSuppliedItemDialog();
            addSuppliedItemWindow.ShowDialog();

            if (addSuppliedItemWindow.DialogResult != true) return;
        }

        private void ItemRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SupplierDataGrid.SelectedItem == null) return;
            if (ListBox.SelectedItem == null) return;
            RefreshListBox();
        }

        #endregion
    }
}