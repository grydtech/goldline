using System;
using System.Windows;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            var customerHandler = new CustomerHandler();
            if (NameTextBox.Text == "" || ContactInfoTextBox.Text == "" || !customerHandler.IsNicValid(NicTextBox.Text))
            {
                MessageBox.Show("Please make sure your inputs are valid");
                return;
            }
            var customer = new Customer(
                NameTextBox.Text,
                ContactInfoTextBox.Text,
                NicTextBox.Text,
                decimal.Parse(DueTextBox.Text));
            try
            {
                customerHandler.AddCustomer(customer);
                MessageBox.Show("Customer added successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            ContactInfoTextBox.Text = "";
            NicTextBox.Text = "";
            DueTextBox.Text = "";
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}