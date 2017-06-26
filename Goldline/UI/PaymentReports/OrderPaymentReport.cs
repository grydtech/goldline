using System;
using System.Windows.Forms;
using Core.Domain.Handlers;

namespace Goldline.UI.PaymentReports
{
    public partial class OrderPaymentReport : Form
    {
        private readonly DateTime _startDate, _endDate;

        public OrderPaymentReport(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            InitializeComponent();
        }

        private void CustomerPaymentReport_Load(object sender, EventArgs e)
        {
            var customerPayments = new OrderPaymentHandler().GetPayments();
            CustomerPaymentBindingSource.DataSource = customerPayments;
            reportViewer.RefreshReport();
        }
    }
}