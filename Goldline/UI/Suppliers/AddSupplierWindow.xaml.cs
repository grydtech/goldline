using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;
using Core.Domain.Model.Suppliers;

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
        }

        private void RefreshListBox()
        {
            ListBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            ListBox?.Items.Refresh();
        }

        private void ItemRemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedItem == null) return;
            RefreshListBox();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Supplier.Name = NameTextBox.Text;
            Supplier.Contact = ContactInfoTextBox.Text;
        }

        private void DiscardButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}