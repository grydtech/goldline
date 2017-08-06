using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Employees;
using Goldline.UI.Security;

namespace Goldline.UI.Employees
{
    /// <summary>
    ///     Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow
    {
        private readonly EmployeeHandler _employeeHandler;

        public EmployeeWindow()
        {
            _employeeHandler = new EmployeeHandler();
            EmployeeSource = _employeeHandler.GetEmployees();
            InitializeComponent();
        }

        public IEnumerable<Employee> EmployeeSource { get; set; }

        private void ManageUserAccessButton_OnClick(object sender, RoutedEventArgs e)
        {
            var employee = EmployeeDataGrid.SelectedItem as Employee;
            if (employee == null) return;
            new UserAccessDialog(employee).ShowDialog();
            ReloadDataGrid();
        }

        #region Button Operations

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            // revert to original employee information
            ReloadDataGrid();
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactInfoTextBox.Text == "")
            {
                MessageBox.Show("You cannot have empty contact information");
            }
            else
            {
                try
                {
                    foreach (Employee employee in EmployeeDataGrid.Items)
                        _employeeHandler.UpdateEmployee(employee);
                    MessageBox.Show("All Employees update successfully");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                ReloadDataGrid();
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.AddEmployeeDialog().ShowDialog();
            ReloadDataGrid();
        }

        private void ToggleEmployeeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem == null) MessageBox.Show(@"No Employee Selected");
            else if (MessageBox.Show(this, @"Are You Sure?", "Confirmation", MessageBoxButton.YesNo) ==
                     MessageBoxResult.Yes)
                _employeeHandler.UpdateEmployee((Employee) EmployeeDataGrid.SelectedItem,
                    isActive: ((ToggleButton) sender).IsChecked == true);
        }

        private void PaymentsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new EmployeePaymentWindow().ShowDialog();
        }

        #endregion

        #region UI Code Behind

        #region EmployeeDataGrid Updates

        private void ReloadDataGrid()
        {
            // Update Data Grid with new set of employees
            EmployeeSource = _employeeHandler.GetEmployees(SearchTextBox.Text);
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

        #region SearchComboBox Behaviour

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadDataGrid();
        }

        #endregion

        #endregion
    }
}