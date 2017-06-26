using System.Windows;
using System.Windows.Input;
using Core.Domain.Enums;
using Core.Domain.Handlers;

namespace Goldline.UI.Security
{
    /// <summary>
    ///     Interaction logic for AuthenticationDialog.xaml
    /// </summary>
    public partial class AuthenticationDialog : Window
    {
        private readonly SecurityHandler _securityHandler;

        public AuthenticationDialog()
        {
            _securityHandler = new SecurityHandler();
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
            var user = _securityHandler.TryAuthentication(UsernameTextBox.Text, PasswordBox.Password);
            if (user != null)
            {
                if (user.AccessMode == AccessMode.Manager) DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Not Authorized", "Error", MessageBoxButton.OK);
            }
        }
    }
}