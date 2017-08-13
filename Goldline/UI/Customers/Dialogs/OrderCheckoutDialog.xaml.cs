using System;
using System.Globalization;
using System.Windows;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Goldline.UI.Invoices;

namespace Goldline.UI.Customers.Dialogs
{
    /// <summary>
    ///     Interaction logic for OrderCheckoutDialog.xaml
    /// </summary>
    public partial class OrderCheckoutDialog : Window
    {
        private readonly Order _order;
        private readonly OrderHandler _orderHandler;
        private readonly OrderPaymentHandler _orderPaymentHandler;

        public OrderCheckoutDialog(Order order)
        {
            _order = order;
            _orderHandler = new OrderHandler();
            _orderPaymentHandler = new OrderPaymentHandler();
            InitializeComponent();

            TotalTextBox.Text = _order.Amount.ToString(CultureInfo.InvariantCulture);
            CustomerComboBox.ItemsSource = new CustomerHandler().GetCustomers();
            CustomerComboBox.GetBindingExpression(PersonComboBox.ItemsSourceProperty)?.UpdateTarget();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OrderCheckoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            _order.CustomerId = (CustomerComboBox?.SelectedItem as Customer)?.Id;
            _order.Note = NoteTextBox.Text;
            var payment = PaymentTextBox.Text == "" ? _order.Amount : decimal.Parse(PaymentTextBox.Text);

            try
            {
                _orderHandler.AddOrder(_order);
                if (_order.Id == null) throw new ArgumentNullException(nameof(_order), @"Order Id Not assigned");
                _orderPaymentHandler.AddPayment(new OrderPayment(_order.Id.Value, payment));
                GenerateInvoice();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
                DialogResult = false;
            }
            finally
            {
                Close();
            }
        }

        public void GenerateInvoice()
        {
            new OrderInvoice(_order).Show();
        }
    }
}