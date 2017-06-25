using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for CustomerPaymentWindow.xaml
    /// </summary>
    public partial class CustomerPaymentWindow : Window
    {
        private readonly OrderPaymentHandler _orderPaymentHandler;

        public CustomerPaymentWindow()
        {
            _orderPaymentHandler = new OrderPaymentHandler();
            CustomerSource = new CustomerHandler().GetCustomers();

            InitializeComponent();
        }

        public IEnumerable<OrderPayment> CustomerPaymentSource { get; set; }
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

                var cs = new OrderPayment((uint) SelectedCustomer.Id, decimal.Parse(AmountTextBox.Text),
                    NoteTextBox.Text);
                try
                {
                    _orderPaymentHandler.AddPayment(cs);
                    MessageBox.Show("Payment recorded successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    AmountTextBox.Text = "";
                    CustomerSource = new CustomerHandler().GetCustomers();
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
                : _orderPaymentHandler.GetPayments(SelectedCustomer.Id);
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