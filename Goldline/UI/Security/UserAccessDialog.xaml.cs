using System;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Employees;

namespace Goldline.UI.Security
{
    /// <summary>
    ///     Interaction logic for UserAccessDialog.xaml
    /// </summary>
    public partial class UserAccessDialog : Window
    {
        private readonly Employee _employee;
        private readonly SecurityHandler _uaHandler;
        private readonly User _user;
        private bool _isExpanded;
        private bool? _isUserNameUnique;

        public UserAccessDialog(Employee employee)
        {
            InitializeComponent();
            _employee = employee;

            // Exception handling
            if (_employee.Id == null) throw new ArgumentNullException(nameof(employee.Id), @"Employee Id is null");

            // Initialize variables
            _uaHandler = new SecurityHandler();
            _user = _uaHandler.GetUser((uint) _employee.Id);

            if (_employee.AccessMode != AccessMode.None && _user == null)
                throw new ArgumentNullException(nameof(_user), @"Employee type is user but no user found");

            #region Initialize UI

            EmployeeIdTextBox.Text = _employee.Id.ToString();
            NameTextBox.Text = _employee.Name;
            UserTypeComboBox.ItemsSource = Enum.GetNames(typeof(AccessMode));
            if (_employee.AccessMode == AccessMode.None) return;

            // If employee is already a user, make user access expander expanded and disable editing username
            ProvideUserAccessExpander.IsExpanded = true;
            UserNameTextBox.IsEnabled = false;
            UserNameTextBox.Text = _user.Username;
            UserTypeComboBox.SelectedItem = _user.AccessMode.ToString();
            CheckAvailabilityButton.IsEnabled = false;

            #endregion
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            var accessMode = (AccessMode)UserTypeComboBox.SelectedIndex;
            // Close if no changes detected
            if (!_isExpanded || _user?.AccessMode==accessMode)
            {
                MessageBox.Show("No changes to be done");
                Close();
                return;
            }

            #region Username validation for regular employees

            if (_employee.AccessMode == AccessMode.None)
            {
                if (UserNameTextBox.Text == "") MessageBox.Show("One or more fields are empty");
                if (_isUserNameUnique == null) CheckIfUserNameAvailable();
                if (_isUserNameUnique == false)
                    MessageBox.Show("This username is already taken. Please choose another");
            }

            #endregion
            
            if (_employee.AccessMode != AccessMode.None)
            {
                // should update existing employee if there are any changes
                _uaHandler.UpdateUserAccess(_user, accessMode);
                MessageBox.Show("User access updated successfully");
            }
            else
            {
                // should add new user to database
                _uaHandler.AddUserAccess(_employee, accessMode, UserNameTextBox.Text);
                MessageBox.Show("User AccessMode provided Successfully");
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckAvailabilityButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Verify if username is available
            if (!String.IsNullOrWhiteSpace(UserNameTextBox.Text))
            {
                CheckIfUserNameAvailable();

                if (_isUserNameUnique == false) MessageBox.Show("This UserName is already taken. Please choose another");
                if (_isUserNameUnique == true) MessageBox.Show("This username is available!");

            }
            else
            {
                MessageBox.Show("Please enter the Username");
            }
        }

        private void CheckIfUserNameAvailable()
        {
            _isUserNameUnique = _uaHandler.IsUsernameAvailable(UserNameTextBox.Text);
        }

        private void UserNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _isUserNameUnique = null;
        }

        #region ProvideUserAccessExpander Behaviour

        private void ProvideUserAccessExpander_Expanded(object sender, RoutedEventArgs e)
        {
            _isExpanded = true;
        }

        private void ProvideUserAccessExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (_employee.AccessMode != AccessMode.None)
            {
                MessageBox.Show("You cannot remove user access for existing users!");
                ((Expander) sender).IsExpanded = true;
                e.Handled = true;
            }
            else
            {
                _isExpanded = false;
            }
        }

        #endregion
    }
}