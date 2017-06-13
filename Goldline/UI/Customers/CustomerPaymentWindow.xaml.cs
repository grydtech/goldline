using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Model.Handlers;
using Core.Model.Payments;
using Core.Model.Persons;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for CustomerPaymentWindow.xaml
    /// </summary>
    public partial class CustomerPaymentWindow : Window
    {
        private readonly CustomerPaymentHandler _customerPaymentHandler;

        public CustomerPaymentWindow()
        {
            _customerPaymentHandler = new CustomerPaymentHandler();
            CustomerSource = new CustomerHandler().GetAllCustomers();

            InitializeComponent();
        }

        public IEnumerable<CustomerPayment> CustomerPaymentSource { get; set; }
        public IEnumerable<Customer> CustomerSource { get; set; }

        public Customer SelectedCustomer => CustomerComboBox.SelectedItem as Customer;

        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDataGrid();
            UpdateButtonEnabled();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)

        {
            if (AmountTextBox.Text == "0" || AmountTextBox.Text == "")
            {
                MessageBox.Show("Please enter a value greater than 0", "GOLDLINE", MessageBoxButton.OK);
            }
            else if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a customer", "GOLDLINE", MessageBoxButton.OK);
            }
            else
            {
                if (SelectedCustomer.Id == null) return;

                var cs = new CustomerPayment(decimal.Parse(AmountTextBox.Text), NoteTextBox.Text,
                    (uint) SelectedCustomer.Id);
                try
                {
                    _customerPaymentHandler.AddNewCustomerPayment(cs);
                    MessageBox.Show("Payment recorded successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    AmountTextBox.Text = "";
                    CustomerSource = new CustomerHandler().GetAllCustomers();
                    RefreshCustomerComboBox();
                    RefreshDataGrid();
                }
            }
        }

        private void RefreshCustomerComboBox()
        {
            CustomerComboBox.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            CustomerComboBox.Items.Refresh();
        }

        #region UI Code Behind

        #region EmployeeDataGrid Updates

        private void RefreshDataGrid()
        {
            CustomerPaymentSource = SelectedCustomer == null
                ? null
                : _customerPaymentHandler.GetCustomerPayments(SelectedCustomer);
            CustomerDataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            CustomerDataGrid.Items.Refresh();
        }

        #endregion

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (CustomerDataGrid.SelectedIndex < CustomerDataGrid.Items.Count - 1)
                        CustomerDataGrid.SelectedIndex++;
                    break;
                case Key.Up:
                    if (CustomerDataGrid.SelectedIndex > 0) CustomerDataGrid.SelectedIndex--;
                    break;
            }
        }

        #endregion

        private void UpdateButtonEnabled()
        {
            PayButton.IsEnabled = SelectedCustomer != null;
        }

        #endregion
    }
}