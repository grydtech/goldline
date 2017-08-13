using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Employees;

namespace Goldline.UI.Employees
{
    /// <summary>
    ///     Interaction logic for EmployeePaymentWindow.xaml
    /// </summary>
    public partial class EmployeePaymentWindow
    {
        private readonly EmployeeHandler _employeeHandler;
        private readonly EmployeePaymentHandler _employeePaymentHandler;

        public EmployeePaymentWindow()
        {
            _employeeHandler = new EmployeeHandler();
            _employeePaymentHandler = new EmployeePaymentHandler();
            EmployeeSource = _employeeHandler.GetEmployees(isLastPaymentDateLoaded: true);
            InitializeComponent();
        }

        public IEnumerable<Employee> EmployeeSource { get; set; }

        private void EmployeeDataGrid_OnRowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem == null) return;
            if (((Employee) EmployeeDataGrid.SelectedItem).EmployeePayments != null) return;
            _employeeHandler.LoadEmployeePayments((Employee) EmployeeDataGrid.SelectedItem);
            EmployeeDataGrid.Items.Refresh();
        }

        #region Button Operations

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            var employee = (Employee) EmployeeDataGrid.SelectedItem;
            if (employee?.Id == null)
            {
                MessageBox.Show("Please Enter Payment Details", "GOLDLINE", MessageBoxButton.OK);
                return;
            }
            if (AmountTextBox.Text != "" && ReasonTextBox.Text != "")
            {
                decimal amount;
                if (decimal.TryParse(AmountTextBox.Text, out amount))
                {
                    var employeePayment = new EmployeePayment(employee.Id.Value, decimal.Parse(AmountTextBox.Text),
                        ReasonTextBox.Text);
                    _employeePaymentHandler.AddPayment(employeePayment);
                    ReloadDataGrid();
                    AmountTextBox.Text = "";
                    ReasonTextBox.Text = "Salary";
                    MessageBox.Show("Payment Recorded Successfully", "GOLDLINE", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Please Enter Valid Amount", "GOLDLINE", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Please Enter Payment Details", "GOLDLINE", MessageBoxButton.OK);
            }
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            AmountTextBox.Text = "";
            ReloadDataGrid();
        }

        private void ButtonGenerateReport_OnClick(object sender, RoutedEventArgs e)
        {
            new TransactionReportWindow().Show();
        }

        #endregion

        #region UI Code Behind

        #region EmployeeDataGrid Updates

        private void ReloadDataGrid()
        {
            // Update Data Grid with new set of employees
            EmployeeSource = _employeeHandler.GetEmployees(isLastPaymentDateLoaded: true);
            EmployeeDataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }

        #endregion

        #region Window Keydown Handling

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (EmployeeDataGrid.SelectedIndex < EmployeeDataGrid.Items.Count - 1)
                        EmployeeDataGrid.SelectedIndex++;
                    e.Handled = true;
                    AmountTextBox.Text = "";
                    break;
                case Key.Up:
                    if (EmployeeDataGrid.SelectedIndex > 0) EmployeeDataGrid.SelectedIndex--;
                    e.Handled = true;
                    AmountTextBox.Text = "";
                    break;
            }
        }

        #endregion

        #endregion
    }
}