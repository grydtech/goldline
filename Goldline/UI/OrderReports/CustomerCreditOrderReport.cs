using System;
using System.Windows.Forms;
using Core.Domain.Handlers;

namespace Goldline.UI.OrderReports
{
    public partial class CustomerCreditOrderReport : Form
    {
        private readonly DateTime _startDate, _endDate;

        public CustomerCreditOrderReport(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            InitializeComponent();
        }

        private void CustomerCreditOrderReport_Load(object sender, EventArgs e)
        {
            var creditOrders = new OrderHandler().GetRecentCustomerOrders(false, false, true);
            CustomerOrderBindingSource.DataSource = creditOrders;
            reportViewer.RefreshReport();
        }
    }
}