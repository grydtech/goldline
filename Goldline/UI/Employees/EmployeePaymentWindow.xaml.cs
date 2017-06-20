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
            EmployeeSource = _employeeHandler.GetAllEmployees();
            InitializeComponent();
        }

        public IEnumerable<Employee> EmployeeSource { get; set; }

        #region Button Operations

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            var employeePayment = new EmployeePayment(
                decimal.Parse(AmountTextBox.Text),
                NoteTextBox.Text,
                (uint) ((Employee) EmployeeDataGrid.SelectedItem).Id);

            _employeePaymentHandler.AddNewEmployeePayment(employeePayment);
            ReloadDataGrid();
        }

        private void ReversePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadDataGrid();
            MessageBox.Show("This feature is not implemented yet");
        }

        private void SummaryButton_OnClick(object sender, RoutedEventArgs e)
        {
            new TransactionReportWindow().Show();
        }

        #endregion

        #region UI Code Behind

        #region EmployeeDataGrid Updates

        private void ReloadDataGrid()
        {
            // Update Data Grid with new set of employees
            EmployeeSource = _employeeHandler.GetAllEmployees();
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
                    break;
                case Key.Up:
                    if (EmployeeDataGrid.SelectedIndex > 0) EmployeeDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        #endregion

        #endregion
    }
}