using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Employees;

namespace Goldline.UI.Employees.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddEmployeeDialog.xaml
    /// </summary>
    public partial class AddEmployeeDialog : Window
    {
        private readonly EmployeeHandler _employeeHandler;
        private readonly SecurityHandler _securityHandler;
        private bool _isUser;
        private bool? _isUserNameUnique;

        public AddEmployeeDialog()
        {
            InitializeComponent();
            AccessModeComboBox.ItemsSource = Enum.GetNames(typeof(AccessMode)).ToList().GetRange(1, 3);
            _employeeHandler = new EmployeeHandler();
            _securityHandler = new SecurityHandler();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (NameTextBox.Text == "" || ContactInfoTextBox.Text == "" || _isUser && UserNameTextBox.Text == "")
            {
                MessageBox.Show("One or more fields are empty");
                return;
            }

            if (_isUser && _isUserNameUnique == null)
                _isUserNameUnique = _securityHandler.IsUsernameAvailable(UserNameTextBox.Text);

            if (_isUser && _isUserNameUnique == false)
            {
                MessageBox.Show("This username is already taken. Please choose another");
                return;
            }

            #endregion

            // Should insert employee into database before giving user access.
            var employee = new Employee(NameTextBox.Text, ContactInfoTextBox.Text, true, AccessMode.None);

            try
            {
                _employeeHandler.AddEmployee(employee);
                MessageBox.Show("Employee added successfully");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            if (_isUser)
                try
                {
                    new SecurityHandler().AddUserAccess(employee, (AccessMode) AccessModeComboBox.SelectedIndex + 1,
                        UserNameTextBox.Text);
                    MessageBox.Show("User access provided successfully");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            Close();
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            ContactInfoTextBox.Text = "";
            _isUser = false;
            ProvideUserAccessExpander.IsExpanded = false;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VerifyButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Verify if username is available
            if (!string.IsNullOrWhiteSpace(UserNameTextBox.Text))
                MessageBox.Show(_securityHandler.IsUsernameAvailable(UserNameTextBox.Text)
                    ? "This username is available!"
                    : "This UserName is already taken. Please choose another");
            else
                MessageBox.Show("Please enter a username");
        }

        private void UserNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // reset any indications showing username is unique, etc
        }

        #region ProvideUserAccessExpander Behaviour

        private void ProvideUserAccessExpander_Expanded(object sender, RoutedEventArgs e)
        {
            _isUser = true;
        }

        private void ProvideUserAccessExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            _isUser = false;
        }

        #endregion
    }
}