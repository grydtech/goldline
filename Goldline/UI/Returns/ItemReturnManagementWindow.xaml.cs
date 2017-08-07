using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Returns
{
    /// <summary>
    ///     Interaction logic for ItemReturnManagementWindow.xaml
    /// </summary>
    public partial class ItemReturnManagementWindow
    {
        private readonly string _defaultText = "Enter your text here..";
        private readonly ItemReturnHandler _itemReturnHandler;
        private IEnumerable<ItemReturn> _returnedItemSource;
        private bool? _isHandled;

        public ItemReturnManagementWindow()
        {
            InitializeComponent();
            _itemReturnHandler = new ItemReturnHandler();
            FilterComboBox.ItemsSource = new[] {"All", "Handled", "Pending"};
        }

        public void Refresh()
        {
            _returnedItemSource = _itemReturnHandler.GetItemReturns(SearchTextBox?.Text, _isHandled);
            InventoryDataGrid.ItemsSource = _returnedItemSource;
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

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox?.Text != _defaultText)
                Refresh();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isHandled = FilterComboBox.SelectedIndex == 0 ? (bool?) null : FilterComboBox.SelectedIndex == 1;
            Refresh();
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
    }
}