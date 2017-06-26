using System;
using System.Windows.Forms;
using Core.Domain.Handlers;

namespace Goldline.UI.OrderReports
{
    public partial class CreditOrderReport : Form
    {
        private readonly DateTime _startDate, _endDate;

        public CreditOrderReport(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            InitializeComponent();
        }

        private void CustomerCreditOrderReport_Load(object sender, EventArgs e)
        {
            var creditOrders = new OrderHandler().GetOrders();
            CustomerOrderBindingSource.DataSource = creditOrders;
            reportViewer.RefreshReport();
        }
    }
}