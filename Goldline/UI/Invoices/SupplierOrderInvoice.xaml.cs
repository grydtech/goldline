using System.Windows;
using System.Windows.Input;
using Core.Model.Orders;
using Core.Model.Persons;

namespace Goldline.UI.Invoices
{
    /// <summary>
    ///     Interaction logic for SupplierOrderInvoice.xaml
    /// </summary>
    public partial class SupplierOrderInvoice : Window
    {
        public SupplierOrderInvoice(SupplierOrder supplierOrder, Supplier supplier)
        {
            InitializeComponent();
            IsVerified = false;
            OrderEntriesDataGrid.ItemsSource = supplierOrder.OrderEntries;
            IdLabel.Content = "SupplierOrder: " + supplierOrder.Id;
            SupplierNameLabel.Content = supplier.Name;
            ContactLabel.Content = supplier.Contact;
            TotalLabel.Content = supplierOrder.Total;
            CashCreditLabel.Content = supplierOrder.Status.ToString();
        }

        public bool IsVerified { get; private set; }

        private void VerifyButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsVerified = true;
            Close();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsVerified = false;
            Close();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Dragging window from the titlebar
            DragMove();
        }
    }
}