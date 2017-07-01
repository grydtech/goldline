using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Goldline.UI.Invoices;

//using log4net;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for OrderCheckoutWindow.xaml
    /// </summary>
    public partial class OrderCheckoutWindow : Window
    {
        private readonly CustomerHandler _customerHandler;
        private readonly OrderHandler _orderHandler;
        private readonly OrderPaymentHandler _orderPaymentHandler;
        // private static readonly ILog log =
        //     LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEnumerable<Customer> _customerMatches;
        private readonly string _searchName;
        private Order _order;

        public OrderCheckoutWindow(Order order)
        {
            _order = order;
            _orderHandler = new OrderHandler();
            _customerHandler = new CustomerHandler();
            _orderPaymentHandler = new OrderPaymentHandler();
            _customerMatches = _customerHandler.GetCustomers();
            InitializeComponent();
            TotalTextBox.Text = _order.Amount.ToString();

            CustomerDataGrid.ItemsSource = _customerMatches;
        }

        // public IEnumerable<Customer> ItemSource { get; set; }
        //public Customer SelectedCustomer { get; set; }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox) sender;
            CustomerDataGrid.ItemsSource = textBox.Text != ""
                ? _customerHandler.GetCustomers(textBox.Text.Trim())
                : _customerHandler.GetCustomers();
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OrderCheckoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            _order.CustomerId = (CustomerDataGrid?.SelectedItem as Customer)?.Id;
            var payment = PaymentTextBox.Text == "" ? _order.Amount : decimal.Parse(PaymentTextBox.Text);
            
            //here AddOrder meth ssgould return bool .THEN only we can generate success msg below
            try
            {
                //_order.Amount = Decimal.Parse(PaymentTextBox.Text);

                _orderHandler.AddOrder(_order);
                if (_order.Id == null) throw new ArgumentNullException(nameof(_order), @"Order Id Not assigned");
                _orderPaymentHandler.AddPayment(new OrderPayment(_order.Id.Value, payment, ""));
                //MessageBox.Show(
                //"Order added successfully. " +
                //"Order Type: Credit. " +
                //"Customer Name: " + SelectedCustomer.Name);
                GenerateInvoice();
                //Close();
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