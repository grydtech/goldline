using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Employees;
using Goldline.UI.Employees.Dialogs;
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
            var employee = (sender as Button)?.Tag as Employee;
            if (employee == null) return;
            var dialogResult = new UserAccessDialog(employee).ShowDialog();
            if (dialogResult == true) ReloadDataGrid();
        }

        #region Button Operations

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            // revert to original employee information
            ReloadDataGrid();
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContactInfoTextBox.Text == string.Empty || NameTextBox.Text == string.Empty)
            {
                MessageBox.Show("You cannot have empty contact information");
            }
            else
            {
                try
                {
                    _employeeHandler.UpdateEmployee((Employee) EmployeeDataGrid.SelectedItem, NameTextBox.Text,
                        ContactInfoTextBox.Text);
                    MessageBox.Show("Updated successfully");
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
            new AddEmployeeDialog().ShowDialog();
            ReloadDataGrid();
        }

        private void ToggleEmployeeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            var togglebutton = sender as ToggleButton;
            var employee = togglebutton?.Tag as Employee;
            if (employee == null) MessageBox.Show(@"No Employee Selected");
            else if (MessageBox.Show(this, @"Are You Sure?", "Confirmation", MessageBoxButton.YesNo) ==
                     MessageBoxResult.Yes)
                _employeeHandler.UpdateEmployee(employee, isActive: ((ToggleButton) sender).IsChecked == true);
            togglebutton?.GetBindingExpression(ToggleButton.IsCheckedProperty)?.UpdateTarget();
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

        #region ProductComboBox Behaviour

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadDataGrid();
        }

        #endregion

        #endregion
    }
}