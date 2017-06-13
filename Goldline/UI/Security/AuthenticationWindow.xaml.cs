using System.Windows;
using System.Windows.Input;
using Core.Model.Handlers;
using Core.Security;

namespace Goldline.UI.Security
{
    /// <summary>
    ///     Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        private readonly UserAccessHandler _userAccessHandler;

        public AuthenticationWindow()
        {
            _userAccessHandler = new UserAccessHandler();
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
            DialogResult = false;
            Close();
        }

        private void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            var user = _userAccessHandler.TryAuthentication(UsernameTextBox.Text, PasswordBox.Password);
            if (user != null)
            {
                if (user.UserType == UserType.Manager) DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Not Authorized", "Error", MessageBoxButton.OK);
            }
        }
    }
}