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
            //ItemSource = _customerMatches;
            InitializeComponent();
            TotalTextBox.Text = _order.Amount.ToString();

            CustomerDataGrid.ItemsSource = _customerMatches;
        }

        // public IEnumerable<Customer> ItemSource { get; set; }
        public Customer SelectedCustomer { get; set; }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (textBox.Text != "")
                CustomerDataGrid.ItemsSource =
                    _customerMatches.Where(customer => customer.Name.Contains(textBox.Text.Trim()));

            //ItemSource = textBox.Text != "" && CustomerDataGrid != null
            //   ? _customerHandler.GetCustomers(textBox.Text)
            //   :_customerHandler.GetCustomers();
            //CustomerDataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }

        private void CustomerSearchTextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            textBox.Text =
                textBox.Text == "" ? _searchName : textBox.Text;
        }

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCustomer = CustomerDataGrid.SelectedItem as Customer;
            if (SelectedCustomer == null) return;

            CustomerIdTextBox.Text = SelectedCustomer.Id.ToString();
            NameTextBox.Text = SelectedCustomer.Name;
            ContactInfoTextBox.Text = SelectedCustomer.Contact;
            NicTextBox.Text = SelectedCustomer.Nic;
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select the relevant customer");
                DialogResult = false;
            }
            else
            {
                // MessageBox.Show("Credit customer verified");
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OrderCheckoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedCustomer != null)
            {
                _order.CustomerId = SelectedCustomer.Id;
            }
            
            //here AddOrder meth ssgould return bool .THEN only we can generate success msg below
            try
            {
                //_order.Amount = Decimal.Parse(PaymentTextBox.Text);

                _orderHandler.AddOrder(_order);
                _orderPaymentHandler.AddPayment(new OrderPayment(_order.Id.Value,
                    Decimal.Parse(PaymentTextBox.Text),""));
                //MessageBox.Show(
                //"Order added successfully. " +
                //"Order Type: Credit. " +
                //"Customer Name: " + SelectedCustomer.Name);
                GenerateInvoice();
                Close();
                DialogResult = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
                DialogResult = false;
                
            }
                
    }

        private void CancelOrderCheckoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public void GenerateInvoice()
        {
            new OrderInvoice(_order).Show();
        }
    }
}