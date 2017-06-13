using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Model;
using Core.Model.Enums;
using Core.Model.Handlers;

namespace Goldline.UI.Returns
{
    /// <summary>
    ///     Interaction logic for ItemReturnManagementWindow.xaml
    /// </summary>
    public partial class ItemReturnManagementWindow : Window
    {
        private readonly string _defaultText = "Enter your text here..";
        private readonly ItemReturnHandler _itemReturnHandler;
        private ReturnCondition? _condition;
        private IEnumerable<ItemReturn> _returnedItemSource;
        private ItemReturn _selectedItemReturn;

        public ItemReturnManagementWindow()
        {
            InitializeComponent();
            _itemReturnHandler = new ItemReturnHandler();
            ComboBox.ItemsSource = Enum.GetNames(typeof(ReturnCondition));
        }

        public void RefreshDataGrid(string text = "%")
        {
            _returnedItemSource = _itemReturnHandler.SearchItemReturns(text, _condition);
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
            {
                RefreshDataGrid(SearchTextBox.Text);
            }
        }

        private void TextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            textBox.Text =
                textBox.Text == _defaultText
                    ? ""
                    : textBox.Text == "" ? _defaultText : textBox.Text;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _condition = (ReturnCondition?) ComboBox.SelectedIndex;
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
            UpdateItemReturn(ReturnCondition.Rejected);
        }

        private void AcceptedButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemReturn(ReturnCondition.Accepted);
        }

        private void CompletedButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemReturn(ReturnCondition.Completed);
        }

        private void UpdateItemReturn(ReturnCondition condition)
        {
            if (_selectedItemReturn == null) return;
            _selectedItemReturn.Condition = condition;
            _itemReturnHandler.UpdateItemReturn(_selectedItemReturn);
            RefreshDataGrid();
        }

        #endregion
    }
}