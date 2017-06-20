using System;
using System.Windows.Forms;
using Core.Domain.Handlers;

namespace Goldline.UI.OrderReports
{
    public partial class SupplierOrderReport : Form
    {
        private DateTime _startDate, _endDate;

        public SupplierOrderReport(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            InitializeComponent();
        }

        private void SupplyOrderReport_Load(object sender, EventArgs e)
        {
            var supplyOrders = new OrderHandler().GetRecentSupplyOrders(false);
            SupplyOrderBindingSource.DataSource = supplyOrders;
            reportViewer.RefreshReport();
        }
    }
}