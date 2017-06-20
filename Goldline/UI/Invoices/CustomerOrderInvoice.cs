using System;
using System.Windows.Forms;
using Core.Domain.Model.Customers;

namespace Goldline.UI.Invoices
{
    public partial class CustomerOrderInvoice : Form
    {
        private readonly Order _order;

        public CustomerOrderInvoice(Order order)
        {
            _order = order;
            InitializeComponent();
        }

        private void CustomerOrderInvoice_Load(object sender, EventArgs e)
        {
            CustomerOrderBindingSource.DataSource = _order;
            reportViewer.RefreshReport();
        }
    }
}