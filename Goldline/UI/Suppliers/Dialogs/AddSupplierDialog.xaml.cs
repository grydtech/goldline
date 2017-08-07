using System.Windows;
using Core.Domain.Handlers;
using Core.Domain.Model.Suppliers;

namespace Goldline.UI.Suppliers.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddSupplierDialog.xaml
    /// </summary>
    public partial class AddSupplierDialog
    {
        public AddSupplierDialog()
        {
            InitializeComponent();
            Supplier = new Supplier();
        }

        public Supplier Supplier { get; set; }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            Supplier.Name = NameTextBox.Text;
            Supplier.Contact = ContactInfoTextBox.Text;
            new SupplierHandler().AddNewSupplier(Supplier);
            MessageBox.Show("Successfully Added Supplier");
            Close();
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            Supplier = new Supplier();
            NameTextBox.Clear();
            ContactInfoTextBox.Clear();
        }
    }
}