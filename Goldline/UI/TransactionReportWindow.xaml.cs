using System;
using System.Reflection;
using System.Windows;
using Goldline.UI.OrderReports;
using Goldline.UI.PaymentReports;
using log4net;

namespace Goldline.UI
{
    /// <summary>
    ///     Interaction logic for TransactionReportWindow.xaml
    /// </summary>
    public partial class TransactionReportWindow : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DateTime _end = DateTime.Today;

        private readonly string[] _itemSource = Enum.GetNames(typeof(ReportType));

        private readonly DateTime _start = DateTime.MinValue;

        public TransactionReportWindow()
        {
            InitializeComponent();
            ResetDatePickers();
            ComboBox.ItemsSource = _itemSource;
        }

        private void ResetDatePickers()
        {
            StartDatePicker.SelectedDate = _start;
            EndDatePicker.SelectedDate = _end;
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate > EndDatePicker.SelectedDate)
            {
                MessageBox.Show("Selected StartDate is Greater Than the EndDate!");
                Log.Debug("StartDate is Greater Than the EndDate");
                return;
            }

            var selectedReportType = (ReportType) ComboBox.SelectedIndex;
            var startDate = (DateTime) StartDatePicker.SelectedDate;
            var endDate = (DateTime) EndDatePicker.SelectedDate;

            switch (selectedReportType)
            {
                case ReportType.CustomerCashOrderReport:
                    new OrderReport(startDate, endDate).ShowDialog();
                    break;
                case ReportType.CustomerCreditOrderReport:
                    new CreditOrderReport(startDate, endDate).ShowDialog();
                    break;
                case ReportType.SupplyOrderReport:
                    new PurchaseReport(startDate, endDate).ShowDialog();
                    break;
                case ReportType.CustomerPaymentReport:
                    new OrderPaymentReport(startDate, endDate).ShowDialog();
                    break;
                case ReportType.EmployeePaymentReport:
                    new EmployeePaymentReport(startDate, endDate).ShowDialog();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedReportType),
                        @"Combobox selected report type is not valid");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}