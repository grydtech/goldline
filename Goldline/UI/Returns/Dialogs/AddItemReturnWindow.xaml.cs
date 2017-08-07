using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Returns.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddItemReturnWindow.xaml
    /// </summary>
    public partial class AddItemReturnWindow : Window
    {
        private readonly ItemReturnHandler _itemReturnHandler;
        private ItemReturnManagementWindow _observer;

        public AddItemReturnWindow()
        {
            _itemReturnHandler = new ItemReturnHandler();
            ItemsSource = new ProductHandler().GetItems();
            CustomerSource = new CustomerHandler().GetCustomers();
            InitializeComponent();
        }

        public IEnumerable<Customer> CustomerSource { get; set; }
        public IEnumerable<Item> ItemsSource { get; set; }

        private bool IsDataInCorrectForm()
        {
            return SearchCustomerComboBox.SelectedItem != null &&
                   (SearchCustomerComboBox.SelectedItem != null || ContactInfoTextBox.Text != string.Empty) &&
                   QuantityTextBox.Text != string.Empty;
        }

        private ItemReturn CreateItemReturnFromData()
        {
            try
            {
                return new ItemReturn(
                    (SearchItemComboBox.SelectedItem as Item)?.Id.GetValueOrDefault() ?? 0,
                    (SearchCustomerComboBox.SelectedItem as Customer)?.Id.GetValueOrDefault() ?? 0,
                    uint.Parse(QuantityTextBox.Text),
                    false,
                    NotesTextBox.Text);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
                return null;
            }
        }

        #region NotifyThereturnedItemsManagement

        public void AddObserver(ItemReturnManagementWindow window)
        {
            _observer = window;
        }

        private void NotifyObserversAndClose()
        {
            if (_observer != null)
                _observer.Refresh();
            else
                new ItemReturnManagementWindow().Show();
            Close();
        }

        #endregion

        #region ButtonClick

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsDataInCorrectForm())
            {
                _itemReturnHandler.AddItemReturn(CreateItemReturnFromData());
                MessageBox.Show("Successfully Added!!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                NotifyObserversAndClose();
            }
            else
            {
                MessageBox.Show("Incomplete Data!!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchItemComboBox.SelectedItem = null;
            SearchCustomerComboBox.SelectedItem = null;
            QuantityTextBox.Text = string.Empty;
            ContactInfoTextBox.Text = string.Empty;
            NotesTextBox.Text = string.Empty;
        }

        private void ClearContactInfoButton_Click(object sender, RoutedEventArgs e)
        {
            ContactInfoTextBox.Text = string.Empty;
        }

        #endregion

        private void SearchCustomerComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContactInfoTextBox.Text != string.Empty || SearchCustomerComboBox.SelectedItem == null) return;
            var contactInfo = (SearchCustomerComboBox.SelectedItem as Customer)?.Contact;
            ContactInfoTextBox.Text = contactInfo;
        }
    }
}