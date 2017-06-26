using System;
using System.Windows.Forms;
using Core.Domain.Handlers;

namespace Goldline.UI.OrderReports
{
    public partial class PurchaseReport : Form
    {
        private DateTime _startDate, _endDate;

        public PurchaseReport(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            InitializeComponent();
        }

        private void SupplyOrderReport_Load(object sender, EventArgs e)
        {
            var supplyOrders = new PurchaseHandler().GetPurchases();
            SupplyOrderBindingSource.DataSource = supplyOrders;
            reportViewer.RefreshReport();
        }
    }
}