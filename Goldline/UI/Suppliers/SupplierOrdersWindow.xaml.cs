using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Suppliers;
using Control = System.Windows.Forms.Control;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for SupplierOrdersWindow.xaml
    /// </summary>
    public partial class SupplierOrdersWindow : Window
    {
        private readonly PurchaseHandler _purchaseHandler;

        public SupplierOrdersWindow()
        {
            _purchaseHandler = new PurchaseHandler();
            PurchaseSource = _purchaseHandler.GetPurchases();
            InitializeComponent();
        }

        public IEnumerable<Purchase> PurchaseSource { get; set; }

        public void RefreshDataGrid()
        {
            PurchaseSource = _purchaseHandler.GetPurchases(note: SearchTextBox.Text);
            PurchasesDataGrid.Items.Refresh();
        }

        public Point GetMousePosition()
        {
            var point = Control.MousePosition;
            return new Point(point.X, point.Y);
        }

        private void ViewSupplyOrders_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (PurchasesDataGrid.SelectedIndex < PurchasesDataGrid.Items.Count - 1)
                        PurchasesDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (PurchasesDataGrid.SelectedIndex > 0) PurchasesDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        private void PurchasesDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PurchasesDataGrid.SelectedItem == null) return;
            var mousePosition = GetMousePosition();
            new SupplierOrderDetailsPopupWindow((Purchase) PurchasesDataGrid.SelectedItem)
            {
                Left = mousePosition.X,
                Top = mousePosition.Y
            }.ShowDialog();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        #region Button Click

        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            if (PurchasesDataGrid.SelectedItem == null) return;
            var result = MessageBox.Show("Confirm Supply Order", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            try
            {
                var purchase = (Purchase) PurchasesDataGrid.SelectedItem;
                if (purchase.Id == null) return;
                _purchaseHandler.DeletePurchase(purchase.Id.Value);
                MessageBox.Show("Successfully Reversed", "Successful", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                RefreshDataGrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Not Reversed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            new AddSupplierOrderWindow().ShowDialog();
            RefreshDataGrid();
        }

        #endregion
    }
}