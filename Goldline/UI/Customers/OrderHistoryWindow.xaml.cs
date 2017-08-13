using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for OrderHistoryWindow.xaml
    /// </summary>
    public partial class OrderHistoryWindow : Window
    {
        private readonly OrderHandler _orderHandler;

        public OrderHistoryWindow()
        {
            _orderHandler = new OrderHandler();
            CustomerSource = new CustomerHandler().GetCustomers();
            InitializeComponent();
            RefreshDataGrid();
        }

        public IEnumerable<Order> OrdersSource { get; set; }
        public IEnumerable<Customer> CustomerSource { get; set; }

        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void ViewDetailsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var order = OrderDataGrid.SelectedItem as Order;
            if (order != null) new OrderDetailsWindow(order).Show();
        }

        #region UI Code Behind

        #region EmployeeDataGrid Updates

        private void RefreshDataGrid()
        {
            OrdersSource = _orderHandler.GetOrders(customerId: (CustomerComboBox.SelectedItem as Customer)?.Id);
            OrderDataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            OrderDataGrid.Items.Refresh();
        }

        #endregion

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (OrderDataGrid.SelectedIndex < OrderDataGrid.Items.Count - 1)
                        OrderDataGrid.SelectedIndex++;
                    break;
                case Key.Up:
                    if (OrderDataGrid.SelectedIndex > 0) OrderDataGrid.SelectedIndex--;
                    break;
            }
        }

        #endregion

        #endregion
    }
}