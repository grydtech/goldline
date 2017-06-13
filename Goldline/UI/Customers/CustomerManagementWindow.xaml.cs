using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Model.Handlers;
using Core.Model.Persons;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for CustomerManagementWindow.xaml
    /// </summary>
    public partial class CustomerManagementWindow : Window
    {
        private readonly CustomerHandler _customerHandler;
        private readonly CustomerPaymentHandler _customerPaymentHandler;

        public CustomerManagementWindow()
        {
            _customerHandler = new CustomerHandler();
            _customerPaymentHandler = new CustomerPaymentHandler();
            CustomerSource = _customerHandler.GetCustomer("");
            InitializeComponent();
            RefreshButtonEnabled();
        }

        public IEnumerable<Customer> CustomerSource { get; set; }

        private Customer SelectedCustomer => CustomerDataGrid.SelectedItem as Customer;

        #region Button Operations

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new AddCustomerWindow().ShowDialog();
            RefreshDataGrid();
            RefreshButtonEnabled();
        }

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)

        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a customer", "GOLDLINE", MessageBoxButton.OK);
            }
            else if (NameTextBox.Text == "" || ContactInfoTextBox.Text == "")
            {
                MessageBox.Show("Please enter valid inputs", "GOLDLINE", MessageBoxButton.OK);
            }
            else
            {
                foreach (var customer in CustomerSource)
                {
                    _customerHandler.UpdateCustomer(customer);
                }
                MessageBox.Show("Changes updated successfully", "GOLDLINE", MessageBoxButton.OK);
            }
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e) // revert to original Customer information
        {
            RefreshDataGrid();
        }


        

        #endregion

        #region UI Code Behind

        #region Refresh UI Components

        private void RefreshButtonEnabled()
        {
            UpdateButton.IsEnabled = SelectedCustomer != null;
            DiscardButton.IsEnabled = SelectedCustomer != null;
        }

        private void RefreshDataGrid()
        {
            CustomerSource = _customerHandler.GetCustomer(SearchTextBox.Text);
            CustomerDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            CustomerDataGrid?.Items.Refresh();
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

        #region Selection and Text Changed Behaviour

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void CustomerDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshButtonEnabled();
        }

        #endregion

        #endregion
    }
}