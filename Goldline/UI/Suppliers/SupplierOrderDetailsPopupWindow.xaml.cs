using System.Windows;
using System.Windows.Input;
using Core.Model.Handlers;
using Core.Model.Orders;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for SupplierOrderDetailsPopupWindow.xaml
    /// </summary>
    public partial class SupplierOrderDetailsPopupWindow : Window
    {
        public SupplierOrderDetailsPopupWindow(Purchase purchase)
        {
            Purchase = purchase;
            new OrderHandler().LoadSupplyOrderEntries(Purchase);

            InitializeComponent();
        }

        public Purchase Purchase { get; }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}