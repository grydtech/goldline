using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for AddSuppliedItemDialog.xaml
    /// </summary>
    public partial class AddSuppliedItemDialog : Window
    {
        private readonly ProductHandler _productHandler;

        public AddSuppliedItemDialog()
        {
            _productHandler = new ProductHandler();
            ItemSource = _productHandler.GetItems();

            InitializeComponent();
        }

        public IEnumerable<Item> ItemSource { get; private set; }
        public Item SelectedItem { get; private set; }

        private void SearchItemTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshInventoryDataGrid();
        }

        private void RefreshInventoryDataGrid()
        {
            ItemSource = _productHandler.GetItems(SearchItemTextBox.Text);
            InventoryDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            InventoryDataGrid?.Items.Refresh();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
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
                case Key.Enter:
                    if (InventoryDataGrid.SelectedItem == null) return;
                    SelectedItem = (Item) InventoryDataGrid.SelectedItem;
                    DialogResult = true;
                    e.Handled = true;
                    Close();
                    break;
                case Key.Escape:
                    DialogResult = false;
                    e.Handled = true;
                    Close();
                    break;
            }
        }
    }
}