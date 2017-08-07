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
    ///     Interaction logic for OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        private readonly OrderHandler _orderHandler;

        // private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly OrderPaymentHandler _orderPaymentHandler;

        public OrderDetailsWindow(Order order)
        {
            Order = order;
            _orderHandler = new OrderHandler();
            _orderPaymentHandler = new OrderPaymentHandler();
            Order.OrderPayments = _orderPaymentHandler.GetPayments(order.Id);
            InitializeComponent();
        }

        public Order Order { get; set; }

        private void ReloadOrderPayments()
        {
            Order.OrderPayments = _orderPaymentHandler.GetPayments(Order.Id);
            OrderPaymentsDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }

        private void CancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrderPaymentsDataGrid.SelectedItem as Order;
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
                _orderHandler.UpdateOrder(selectedOrder, isCancelled: true);
                MessageBox.Show("Successfully Cancelled the order :" + selectedOrder.Id);
                ReloadOrderPayments();
            }
        }

        private void AddPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (OrderPaymentsDataGrid.SelectedIndex < OrderPaymentsDataGrid.Items.Count - 1)
                        OrderPaymentsDataGrid.SelectedIndex++;
                    break;
                case Key.Up:
                    if (OrderPaymentsDataGrid.SelectedIndex > 0) OrderPaymentsDataGrid.SelectedIndex--;
                    break;
            }
        }

        #endregion
    }
}