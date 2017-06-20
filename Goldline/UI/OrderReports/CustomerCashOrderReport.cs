using System;
using System.Windows.Forms;
using Core.Domain.Handlers;

namespace Goldline.UI.OrderReports
{
    public partial class CustomerCashOrderReport : Form
    {
        private readonly DateTime _startDate, _endDate;

        public CustomerCashOrderReport(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            InitializeComponent();
        }

        private void CustomerCashOrderReport_Load(object sender, EventArgs e)
        {
            var cashOrders = new OrderHandler().GetRecentCustomerOrders(false, false, false);
            CustomerOrderBindingSource.DataSource = cashOrders;
            reportViewer.RefreshReport();
        }
    }
}