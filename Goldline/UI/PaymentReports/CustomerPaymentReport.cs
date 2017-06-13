using System;
using System.Windows.Forms;
using Core.Model.Handlers;

namespace Goldline.UI.PaymentReports
{
    public partial class CustomerPaymentReport : Form
    {
        private readonly DateTime _startDate, _endDate;

        public CustomerPaymentReport(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            InitializeComponent();
        }

        private void CustomerPaymentReport_Load(object sender, EventArgs e)
        {
            var customerPayments = new CustomerPaymentHandler().GetMostRecentPayments();
            CustomerPaymentBindingSource.DataSource = customerPayments;
            reportViewer.RefreshReport();
        }
    }
}