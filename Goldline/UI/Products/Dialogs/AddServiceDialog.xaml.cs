using System;
using System.Windows;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Products.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddServiceDialog.xaml
    /// </summary>
    public partial class AddServiceDialog : Window
    {
        public AddServiceDialog()
        {
            InitializeComponent();
            TextBoxServiceName.Focus();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBoxServiceName.Text == string.Empty)
                {
                    MessageBox.Show("Error", "Please Enter a Service Name");
                }
                else
                {
                    var serviceName = TextBoxServiceName.Text;
                    var service = new Service(serviceName);
                    new ProductHandler().AddProduct(service);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}