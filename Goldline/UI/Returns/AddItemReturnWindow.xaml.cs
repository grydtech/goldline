using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Returns
{
    /// <summary>
    ///     Interaction logic for AddItemReturnWindow.xaml
    /// </summary>
    public partial class AddItemReturnWindow : Window
    {
        private readonly CustomerHandler _customerHandler;
        private readonly string _defaultCustomerSearch = "Search Customer...";
        private readonly string _defaultItemSearch = "Search Item...";
        private readonly ItemReturnHandler _itemReturnHandler;

        private readonly ProductHandler _productHandler;

        private IEnumerable<Customer> _customerSource;

        private bool _itemFocused = true;

        private IEnumerable<Item> _itemsSource;
        private ItemReturnManagementWindow _observer;
        private Customer _selectedCustomer;
        private Item _selectedItem;

        public AddItemReturnWindow()
        {
            InitializeComponent();
            _productHandler = new ProductHandler();
            _customerHandler = new CustomerHandler();
            _itemReturnHandler = new ItemReturnHandler();
            _itemsSource = _productHandler.GetItems();
            _customerSource = new CustomerHandler().GetCustomers();
            RefreshInventoryDataGrid();
            RefreshCustomerDataGrid();
        }

        private bool IsDataInCorrectForm()
        {
            if (IdTextBox.Text == "") return false;
            if (ItemNameTextBox.Text == "") return false;
            if (QuantityTextBox.Text == "") return false;
            if (QuantityTextBox.Text == "0") return false;
            return true;
        }

        private ItemReturn CreateItemReturnFromData()
        {
            try
            {
                return new ItemReturn(
                    (uint) _selectedItem.Id,
                    (uint) _selectedCustomer.Id,
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

        private void NotifyObservers()
        {
            if (_observer == null)
                new ItemReturnManagementWindow().Show();
            Close();
        }

        #endregion

        #region RefreshTexBoxData

        private void ClearPropertyBoxes()
        {
            IdTextBox.Text = "";
            ItemNameTextBox.Text = "";
            QuantityTextBox.Text = "";
            NotesTextBox.Text = "";
            CustomerNameTextBox.Text = "";
            CustomerInfoTextBox.Text = "";
            _selectedCustomer = null;
            _selectedItem = null;
        }

        private void RefreshItemName()
        {
            if (_selectedItem == null) return;

            ItemNameTextBox.Text = _selectedItem.Name;
        }

        private void RefreshCustomerInfo()
        {
            if (_selectedCustomer == null) return;

            CustomerNameTextBox.Text = _selectedCustomer.Name;
            CustomerInfoTextBox.Text = _selectedCustomer.Contact;
            CustomerInfoTextBox.IsEnabled = true;
        }

        #endregion

        #region ButtonClick

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsDataInCorrectForm())
            {
                _itemReturnHandler.AddItemReturn(CreateItemReturnFromData());
                if (_selectedCustomer == null) return;
                _selectedCustomer.Contact = CustomerInfoTextBox.Text;
                _customerHandler.UpdateCustomer(_selectedCustomer);
                MessageBox.Show("Successfully Added!!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                NotifyObservers();
            }
            else
            {
                MessageBox.Show("Incomplete Data!!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPropertyBoxes();
            RefreshCustomerDataGrid();
        }

        private void CustomerNameClearButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerNameTextBox.Text = "";
            CustomerInfoTextBox.Text = "";
            CustomerInfoTextBox.IsEnabled = false;
            _selectedCustomer = null;
            RefreshCustomerDataGrid();
        }

        private void CustomerInfoDiscardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer == null) return;
            CustomerInfoTextBox.Text = _selectedCustomer.Contact;
        }

        private void ReturnedItemsManagement_OnClick(object sender, RoutedEventArgs e)
        {
            NotifyObservers();
        }

        #endregion

        #region Search (TextBox & ComboBox & KeyPress)

        #region TextBoxFocus

        private void SearchCustomerTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultCustomerSearch
                    ? ""
                    : textBox.Text == ""
                        ? _defaultCustomerSearch
                        : textBox.Text;
            _itemFocused = false;
        }

        private void SearchCustomerTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultCustomerSearch
                    ? ""
                    : textBox.Text == ""
                        ? _defaultCustomerSearch
                        : textBox.Text;
        }

        private void SearchItemTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultItemSearch
                    ? ""
                    : textBox.Text == ""
                        ? _defaultItemSearch
                        : textBox.Text;
            _itemFocused = true;
        }

        private void SearchItemTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text =
                textBox.Text == _defaultItemSearch
                    ? ""
                    : textBox.Text == ""
                        ? _defaultItemSearch
                        : textBox.Text;
        }

        #endregion

        private void SearchItemTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultItemSearch)
                RefreshInventoryDataGrid(textBox?.Text);
        }


        private void SearchCustomerTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultCustomerSearch)
                RefreshCustomerDataGrid(textBox?.Text);
        }

        #region KeyScroling

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (_itemFocused)
                    {
                        if (InventoryDataGrid.SelectedIndex < InventoryDataGrid.Items.Count - 1)
                            InventoryDataGrid.SelectedIndex++;
                    }
                    else
                    {
                        if (CustomerDataGrid.SelectedIndex < CustomerDataGrid.Items.Count - 1)
                            CustomerDataGrid.SelectedIndex++;
                    }
                    e.Handled = true;
                    break;

                case Key.Up:
                    if (_itemFocused)
                    {
                        if (InventoryDataGrid.SelectedIndex > 0) InventoryDataGrid.SelectedIndex--;
                    }
                    else
                    {
                        if (CustomerDataGrid.SelectedIndex > 0) CustomerDataGrid.SelectedIndex--;
                    }
                    e.Handled = true;
                    break;
            }
        }

        #endregion

        #endregion

        #region DataGrid

        private void LoadItemSource(int dataGridNo, string name = "%")
        {
            if (dataGridNo == 1)
                _itemsSource = _productHandler.GetItems(name);
            else
                _customerSource = _customerHandler.GetCustomers(name);
        }

        private void RefreshInventoryDataGrid(string name = "%")
        {
            LoadItemSource(1, name);
            InventoryDataGrid.ItemsSource = _itemsSource;
        }

        private void RefreshCustomerDataGrid(string name = "%")
        {
            LoadItemSource(2, name);
            CustomerDataGrid.ItemsSource = _customerSource;
        }

        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = ((DataGrid) sender).SelectedItem as Item;
            RefreshItemName();
        }

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCustomer = ((DataGrid) sender).SelectedItem as Customer;
            RefreshCustomerInfo();
        }

        #endregion
    }
}