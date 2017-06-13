using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Model.Handlers;
using Core.Model.Persons;
using Core.Model.Products;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for AddSupplierWindow.xaml
    /// </summary>
    public partial class AddSupplierWindow : Window
    {
        public AddSupplierWindow()
        {
            InitializeComponent();
            Supplier = new Supplier();
        }

        public Supplier Supplier { get; set; }

        private void ItemAddButton_Click(object sender, RoutedEventArgs e)
        {
            var addSuppliedItemWindow = new AddSuppliedItemWindow();
            addSuppliedItemWindow.ShowDialog();

            if (addSuppliedItemWindow.DialogResult != true)
            {
                MessageBox.Show("Not successful");
            }
            else
            {
                if (Supplier.SuppliedItems.Any(suppliedItem => suppliedItem.Id == addSuppliedItemWindow.SelectedItem.Id))
                    MessageBox.Show("Item is already in the list.");
                else
                {
                    Supplier.SuppliedItems.Add(addSuppliedItemWindow.SelectedItem);
                    RefreshListBox();
                }
            }
        }

        private void RefreshListBox()
        {
            ListBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            ListBox?.Items.Refresh();
        }

        private void ItemRemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedItem == null) return;
            Supplier.SuppliedItems.Remove((Item) ListBox.SelectedItem);
            RefreshListBox();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Supplier.Name = NameTextBox.Text;
            Supplier.Contact = ContactInfoTextBox.Text;
            if (Supplier.SuppliedItems.Count == 0)
                MessageBox.Show("You should provide at least one supplied item");
            else
            {
                new SupplierHandler().AddNewSupplier(Supplier);
                Close();
            }
        }

        private void DiscardButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}