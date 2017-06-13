using System.Windows;
using System.Windows.Input;
using Core.Model.Handlers;
using Core.Security;

namespace Goldline.UI.Security
{
    /// <summary>
    ///     Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        private void Grid_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            // Dragging window from the titlebar
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to exit?", "Confirm", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes) Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Try authenticating with given username and password
            var uaHandler = new UserAccessHandler();
            var user = uaHandler.TryAuthentication(UsernameTextBox.Text, PasswordBox.Password);

            // If user returned is null, show error and return
            if (user == null)
            {
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK);
                PasswordBox.Focus();
                PasswordBox.SelectAll();
                return;
            }

            // Else assign it to currentuser and show home window
            User.CurrentUser = user;

            if (User.IsDefaultPassword())
            {
                new ChangePasswordWindow().ShowDialog();
            }

            new HomeWindow().Show();
            Close();
        }
    }
}