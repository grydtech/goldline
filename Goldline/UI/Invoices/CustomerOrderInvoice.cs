using System;
using System.Windows.Forms;
using Core.Model.Orders;

namespace Goldline.UI.Invoices
{
    public partial class CustomerOrderInvoice : Form
    {
        private readonly CustomerOrder _customerOrder;

        public CustomerOrderInvoice(CustomerOrder customerOrder)
        {
            _customerOrder = customerOrder;
            InitializeComponent();
        }

        private void CustomerOrderInvoice_Load(object sender, EventArgs e)
        {
            CustomerOrderBindingSource.DataSource = _customerOrder;
            reportViewer.RefreshReport();
        }
    }
}