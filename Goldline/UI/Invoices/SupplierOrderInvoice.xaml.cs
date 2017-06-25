using System.Windows;
using System.Windows.Input;
using Core.Domain.Model.Suppliers;

namespace Goldline.UI.Invoices
{
    /// <summary>
    ///     Interaction logic for SupplierOrderInvoice.xaml
    /// </summary>
    public partial class SupplierOrderInvoice : Window
    {
        public SupplierOrderInvoice(Purchase purchase, Supplier supplier)
        {
            InitializeComponent();
            IsVerified = false;
            OrderEntriesDataGrid.ItemsSource = purchase.PurchaseItems;
            IdLabel.Content = "Purchase: " + purchase.Id;
            SupplierNameLabel.Content = supplier.Name;
            ContactLabel.Content = supplier.Contact;
            TotalLabel.Content = purchase.Amount;
            CashCreditLabel.Content = purchase.IsSettled.ToString();
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