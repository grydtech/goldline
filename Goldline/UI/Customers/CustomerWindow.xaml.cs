﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Goldline.UI.Customers.Dialogs;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly CustomerHandler _customerHandler;
        private readonly OrderPaymentHandler _customerPaymentHandler;

        public CustomerWindow()
        {
            _customerHandler = new CustomerHandler();
            _customerPaymentHandler = new OrderPaymentHandler();
            CustomerSource = _customerHandler.GetCustomers("");
            InitializeComponent();
            RefreshButtonEnabled();
        }

        public IEnumerable<Customer> CustomerSource { get; set; }

        private Customer SelectedCustomer => CustomerDataGrid.SelectedItem as Customer;

        #region Button Operations

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new AddCustomerDialog().ShowDialog();
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
                    _customerHandler.UpdateCustomer(customer);
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
            CustomerSource = _customerHandler.GetCustomers(SearchTextBox.Text);
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