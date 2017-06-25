using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Returns
{
    /// <summary>
    ///     Interaction logic for ItemReturnManagementWindow.xaml
    /// </summary>
    public partial class ItemReturnManagementWindow : Window
    {
        private readonly string _defaultText = "Enter your text here..";
        private readonly ItemReturnHandler _itemReturnHandler;
        private bool? isHandled;
        private IEnumerable<ItemReturn> _returnedItemSource;
        private ItemReturn _selectedItemReturn;

        public ItemReturnManagementWindow()
        {
            InitializeComponent();
            _itemReturnHandler = new ItemReturnHandler();
            ComboBox.ItemsSource = new[] {"True", "False"};
        }

        public void RefreshDataGrid(string text = "%")
        {
            _returnedItemSource = _itemReturnHandler.GetItemReturns(text, isHandled);
            InventoryDataGrid.ItemsSource = _returnedItemSource;
            _selectedItemReturn = (ItemReturn) InventoryDataGrid.SelectedItem;
        }

        private void ItemReturnsManagement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (InventoryDataGrid.SelectedIndex < InventoryDataGrid.Items.Count - 1)
                        InventoryDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (InventoryDataGrid.SelectedIndex > 0) InventoryDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        #region Action Listeners

        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItemReturn = ((DataGrid) sender).SelectedItem as ItemReturn;
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultText)
                RefreshDataGrid(SearchTextBox.Text);
        }

        private void TextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            textBox.Text =
                textBox.Text == _defaultText
                    ? ""
                    : textBox.Text == ""
                        ? _defaultText
                        : textBox.Text;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isHandled = ComboBox.SelectedIndex == 0;
            RefreshDataGrid();
            SearchTextBox.Text = _defaultText;
        }

        #endregion

        #region Button_Click

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new AddItemReturnWindow();
            newWindow.AddObserver(this);
            newWindow.Show();
        }

        private void RejectedButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AcceptedButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CompletedButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void UpdateItemReturn(bool handled)
        {
            if (_selectedItemReturn == null) return;
            _selectedItemReturn.IsHandled = handled;
            _itemReturnHandler.UpdateItemReturn(_selectedItemReturn);
            RefreshDataGrid();
        }

        #endregion
    }
}