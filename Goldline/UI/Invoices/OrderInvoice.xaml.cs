using System.Windows;
using System.Windows.Input;
using Core.Domain.Model.Customers;

namespace Goldline.UI.Invoices
{
    /// <summary>
    ///     Interaction logic for OrderInvoice.xaml
    /// </summary>
    public partial class OrderInvoice : Window
    {
        public OrderInvoice(Order order)
        {
            InitializeComponent();
            IsVerified = false;
            OrderEntriesDataGrid.ItemsSource = order.OrderItems;
            IdLabel.Content = order.Id;
            CustomerNameLabel.Content = order.CustomerId;
            DateLabel.Content = order.Date;
            TotalLabel.Content = order.Amount;
            CashCreditLabel.Content = order.IsSettled ? "Cash" : "Credit";
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