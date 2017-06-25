using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model;
using Core.Domain.Model.Employees;

namespace Goldline.UI.Security
{
    /// <summary>
    ///     Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
            OldPasswordBox.Focus();

            // If password is default password, set it to oldpasswordbox and disable it
            if (Session.CurrentUser.IsDefaultPassword())
            {
                OldPasswordBox.Password = Session.DefaultPassword;
                OldPasswordBox.IsEnabled = false;
            }
        }

        private void Grid_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            // Dragging window from the titlebar
            DragMove();
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (OldPasswordBox.Password != Session.CurrentUser.Password)
            {
                MessageBox.Show("Please enter your old password correctly");
                return;
            }

            if (NewPasswordBox.Password == "" || RepeatPasswordBox.Password == "")
            {
                MessageBox.Show("Password fields cannot be empty");
                return;
            }

            if (NewPasswordBox.Password != RepeatPasswordBox.Password)
            {
                MessageBox.Show("Passwords do not match. Please try again.");
                return;
            }

            if (NewPasswordBox.Password == OldPasswordBox.Password || Session.CurrentUser.IsDefaultPassword())
            {
                MessageBox.Show("You cannot use this password again");
                return;
            }

            #endregion

            try
            {
                new SecurityHandler().ChangePassword(Session.CurrentUser, NewPasswordBox.Password);
                MessageBox.Show("Password changed successfully");
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ChangePasswordWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // if current user password is default password, give error and stop closing
            if (Session.CurrentUser.IsDefaultPassword())
            {
                MessageBox.Show("Password change is mandatory for security reasons");
                e.Cancel = true;
            }
        }
    }
}