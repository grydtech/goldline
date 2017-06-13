using System;
using System.Windows;
using System.Windows.Controls;
using Core.Model.Enums;
using Core.Model.Handlers;
using Core.Model.Persons;
using Core.Security;

namespace Goldline.UI.Employees
{
    /// <summary>
    ///     Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private readonly EmployeeHandler _employeeHandler;
        private readonly UserAccessHandler _userAccessHandler;
        private bool _isUser;
        private bool? _isUserNameUnique;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            UserTypeComboBox.ItemsSource = Enum.GetNames(typeof(UserType));
            _employeeHandler = new EmployeeHandler();
            _userAccessHandler = new UserAccessHandler();
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
                _isUserNameUnique = _userAccessHandler.IsUsernameAvailable(UserNameTextBox.Text);

            if (_isUser && _isUserNameUnique == false)
            {
                MessageBox.Show("This username is already taken. Please choose another");
                return;
            }

            #endregion

            // Should insert employee into database before giving user access.
            var employee = new Employee(
                NameTextBox.Text,
                ContactInfoTextBox.Text,
                _isUser ? EmployeeType.User : EmployeeType.Regular);

            try
            {
                _employeeHandler.AddNewEmployee(employee);
                MessageBox.Show("Employee added successfully");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            if (_isUser)
                try
                {
                    new UserAccessHandler().AddNewUserAccess(employee, (UserType) UserTypeComboBox.SelectedIndex,
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
            MessageBox.Show(_userAccessHandler.IsUsernameAvailable(UserNameTextBox.Text)
                ? "This username is available!"
                : "This UserName is already taken. Please choose another");
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