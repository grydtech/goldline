using System.Windows;

namespace Goldline.UI
{
    /// <summary>
    ///     Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog(string windowTitle, string prompt)
        {
            WindowTitle = windowTitle;
            Prompt = prompt;
            InitializeComponent();
            ResponseTextBox.Focus();
        }

        public string WindowTitle { get; set; }
        public string Prompt { get; set; }

        public string Response => ResponseTextBox.Text;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}