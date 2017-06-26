using System;
using System.Windows;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;

namespace Goldline.UI.Customers.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddCustomerDialog.xaml
    /// </summary>
    public partial class AddCustomerDialog : Window
    {
        public AddCustomerDialog()
        {
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            var customerHandler = new CustomerHandler();
            var customer = new Customer(
                NameTextBox.Text,
                ContactInfoTextBox.Text,
                NicTextBox.Text);

            if (NameTextBox.Text == "" || ContactInfoTextBox.Text == "" || !customer.IsNicValid())
            {
                MessageBox.Show("Please make sure your inputs are valid");
                return;
            }

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