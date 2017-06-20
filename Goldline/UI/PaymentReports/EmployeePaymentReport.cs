using System;
using System.Windows.Forms;
using Core.Domain.Handlers;

namespace Goldline.UI.PaymentReports
{
    public partial class EmployeePaymentReport : Form
    {
        private readonly DateTime _startDate, _endDate;

        public EmployeePaymentReport(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            InitializeComponent();
        }

        private void EmployeePaymentReport_Load(object sender, EventArgs e)
        {
            var employeePayments = new EmployeePaymentHandler().GetRecentEmployeePayments();
            EmployeePaymentBindingSource.DataSource = employeePayments;
            reportViewer.RefreshReport();
        }
    }
}