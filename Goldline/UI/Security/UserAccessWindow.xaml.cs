﻿using System;
using System.Windows;
using System.Windows.Controls;
using Core.Model.Enums;
using Core.Model.Handlers;
using Core.Model.Persons;
using Core.Security;

namespace Goldline.UI.Security
{
    /// <summary>
    ///     Interaction logic for UserAccessWindow.xaml
    /// </summary>
    public partial class UserAccessWindow : Window
    {
        private readonly Employee _employee;
        private readonly UserAccessHandler _uaHandler;
        private readonly User _user;
        private bool _isExpanded;
        private bool? _isUserNameUnique;

        public UserAccessWindow(Employee employee)
        {
            InitializeComponent();
            _employee = employee;

            // Exception handling
            if (_employee.Id == null) throw new ArgumentNullException(nameof(employee.Id), @"Employee Id is null");

            // Initialize variables
            _uaHandler = new UserAccessHandler();
            _user = _uaHandler.GetUser((uint) _employee.Id);

            if (_employee.EmployeeType == EmployeeType.User && _user == null)
                throw new ArgumentNullException(nameof(_user), @"Employee type is user but no user found");

            #region Initialize UI

            EmployeeIdTextBox.Text = _employee.Id.ToString();
            NameTextBox.Text = _employee.Name;
            UserTypeComboBox.ItemsSource = Enum.GetNames(typeof(UserType));
            if (_employee.EmployeeType != EmployeeType.User) return;

            // If employee is already a user, make user access expander expanded and disable editing username
            ProvideUserAccessExpander.IsExpanded = true;
            UserNameTextBox.IsEnabled = false;
            UserNameTextBox.Text = _user.Username;
            UserTypeComboBox.SelectedItem = _user.UserType.ToString();
            CheckAvailabilityButton.IsEnabled = false;

            #endregion
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Close if no changes detected
            if (!_isExpanded)
            {
                MessageBox.Show("No changes to be done");
                Close();
            }

            #region Username validation for regular employees

            if (_employee.EmployeeType == EmployeeType.Regular)
            {
                if (UserNameTextBox.Text == "") MessageBox.Show("One or more fields are empty");
                if (_isUserNameUnique == null) CheckIfUserNameAvailable();
                if (_isUserNameUnique == false)
                    MessageBox.Show("This username is already taken. Please choose another");
            }

            #endregion

            switch (_employee.EmployeeType)
            {
                case EmployeeType.Regular:
                    // should add new user to database
                    _uaHandler.AddNewUserAccess(_employee, (UserType) UserTypeComboBox.SelectedIndex,
                        UserNameTextBox.Text);
                    MessageBox.Show("User Access provided Successfully");
                    break;
                case EmployeeType.User:
                    // should update existing employee if there are any changes
                    _uaHandler.UpdateUserAccess(
                        _user,
                        (UserType) UserTypeComboBox.SelectedIndex);
                    MessageBox.Show("User access updated successfully");
                    break;
            }
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckAvailabilityButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Verify if username is available
            CheckIfUserNameAvailable();

            if (_isUserNameUnique == false) MessageBox.Show("This UserName is already taken. Please choose another");
            if (_isUserNameUnique == true) MessageBox.Show("This username is available!");
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
            if (_employee.EmployeeType == EmployeeType.User)
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