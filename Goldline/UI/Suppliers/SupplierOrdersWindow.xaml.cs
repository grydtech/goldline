using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Model.Handlers;
using Core.Model.Orders;
using Control = System.Windows.Forms.Control;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for SupplierOrdersWindow.xaml
    /// </summary>
    public partial class SupplierOrdersWindow : Window
    {
        private readonly OrderHandler _orderHandler;

        public SupplierOrdersWindow()
        {
            _orderHandler = new OrderHandler();
            SupplyOrderSource = _orderHandler.GetSupplyOrders("");
            InitializeComponent();
        }

        public IEnumerable<SupplierOrder> SupplyOrderSource { get; set; }

        public void RefreshDataGrid()
        {
            SupplyOrderSource = _orderHandler.GetSupplyOrders(SearchTextBox.Text);
            SupplyOrdersDataGrid.Items.Refresh();
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
                    if (SupplyOrdersDataGrid.SelectedIndex < SupplyOrdersDataGrid.Items.Count - 1)
                        SupplyOrdersDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (SupplyOrdersDataGrid.SelectedIndex > 0) SupplyOrdersDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        private void SupplyOrdersDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SupplyOrdersDataGrid.SelectedItem == null) return;
            var mousePosition = GetMousePosition();
            new SupplierOrderDetailsPopupWindow((SupplierOrder) SupplyOrdersDataGrid.SelectedItem)
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
            if (SupplyOrdersDataGrid.SelectedItem == null) return;
            var result = MessageBox.Show("Confirm Supply Order", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            try
            {
                _orderHandler.CancelSupplyOrder((SupplierOrder) SupplyOrdersDataGrid.SelectedItem);
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