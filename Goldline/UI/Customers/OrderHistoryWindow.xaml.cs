﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Goldline.UI.Security;

//using log4net;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for OrderHistoryWindow.xaml
    /// </summary>
    public partial class OrderHistoryWindow : Window
    {
        // private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly OrderHandler _orderHandler;

        public OrderHistoryWindow()
        {
            _orderHandler = new OrderHandler();
            Orders = _orderHandler.GetOrders();
            InitializeComponent();
        }

        public IEnumerable<Order> Orders { get; set; }

        private void OrdersDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshOrderEntriesDataGrid();
        }

        private void RefreshOrderEntriesDataGrid()
        {
            OrderEntriesDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            OrderEntriesDataGrid?.Items.Refresh();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshOrdersDataGrid();
        }

        private void RefreshOrdersDataGrid()
        {
            // Search for orders by note text
            Orders = _orderHandler.GetOrders(note: SearchTextBox.Text);
            OrdersDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            OrdersDataGrid?.Items.Refresh();
        }

        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersDataGrid.SelectedItem as Order;
            if (selectedOrder == null)
            {
                MessageBox.Show("Please select and verify an order to continue");
            }
            else
            {
                var msgBoxResult = MessageBox.Show("Do you  want to reverse order "
                                                   + selectedOrder.Id + " "
                                                   + selectedOrder.Date,
                    "Confirmation",
                    MessageBoxButton.YesNo);
                if (msgBoxResult != MessageBoxResult.Yes) return;

                var authWindow = new AuthenticationDialog();
                authWindow.ShowDialog();
                if (authWindow.DialogResult != true) return;

                selectedOrder.IsCancelled = true;
                _orderHandler.UpdateOrder(selectedOrder);

                MessageBox.Show("Successfully Reversed the order :" + selectedOrder.Id);
                RefreshOrdersDataGrid();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (OrdersDataGrid.SelectedIndex < OrdersDataGrid.Items.Count - 1)
                        OrdersDataGrid.SelectedIndex++;
                    break;
                case Key.Up:
                    if (OrdersDataGrid.SelectedIndex > 0) OrdersDataGrid.SelectedIndex--;
                    break;
            }
        }

        #endregion
    }
}