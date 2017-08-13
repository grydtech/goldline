using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;
using Goldline.UI.Returns.Dialogs;

namespace Goldline.UI.Returns
{
    /// <summary>
    ///     Interaction logic for ItemReturnManagementWindow.xaml
    /// </summary>
    public partial class ItemReturnManagementWindow
    {
        private readonly ItemReturnHandler _itemReturnHandler;
        private bool? _isHandled;
        private uint? _itemId;
        private IEnumerable<ItemReturn> _returnedItemSource;

        public ItemReturnManagementWindow()
        {
            InitializeComponent();
            _itemReturnHandler = new ItemReturnHandler();
            ItemComboBox.ItemsSource = new ProductHandler().GetItems();
            FilterComboBox.ItemsSource = new[] {"All", "Handled", "Pending"};
        }

        public void Refresh()
        {
            _returnedItemSource = _itemReturnHandler.GetItemReturns(SearchTextBox?.Text, _itemId, _isHandled);
            InventoryDataGrid.ItemsSource = _returnedItemSource;
            InventoryDataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
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

        #region Button_Click

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new AddItemReturnWindow();
            newWindow.AddObserver(this);
            newWindow.Show();
        }

        #endregion

        private void ToggleItemReturnHandled_OnClick(object sender, RoutedEventArgs e)
        {
            var togglebutton = sender as ToggleButton;
            var itemReturn = togglebutton?.Tag as ItemReturn;
            if (itemReturn == null) MessageBox.Show(@"No Item Return Selected");
            else if (MessageBox.Show(this, @"Are You Sure you want to change the Status?", "Confirmation",
                         MessageBoxButton.YesNo) ==
                     MessageBoxResult.Yes)
                _itemReturnHandler.UpdateItemReturn(itemReturn, isHandled: ((ToggleButton) sender).IsChecked == true);
            togglebutton?.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateTarget();
        }

        private void ItemComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _itemId = (ItemComboBox.SelectedItem as Item)?.Id;
            Refresh();
        }

        #region Action Listeners

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isHandled = FilterComboBox.SelectedIndex == 0 ? (bool?) null : FilterComboBox.SelectedIndex == 1;
            Refresh();
        }

        #endregion
    }
}