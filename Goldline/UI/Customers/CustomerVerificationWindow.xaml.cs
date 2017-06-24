using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;

//using log4net;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for CustomerVerificationWindow.xaml
    /// </summary>
    public partial class CustomerVerificationWindow : Window
    {
        private readonly CustomerHandler _customerHandler;
        // private static readonly ILog log =
        //     LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEnumerable<Customer> _customerMatches;
        private readonly string _searchName;

        public CustomerVerificationWindow()
        {
            _customerHandler = new CustomerHandler();
            _customerMatches = CustomerHandler.GetCustomers();
            //    ItemSource = _customerMatches;
            InitializeComponent();

            CustomerDataGrid.ItemsSource = _customerMatches;
        }

        // public IEnumerable<Customer> ItemSource { get; set; }
        public Customer SelectedCustomer { get; set; }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (textBox.Text != "")
                CustomerDataGrid.ItemsSource =
                    _customerMatches.Where(customer => customer.Name.Contains(textBox.Text.Trim()));

            //ItemSource = textBox.Text != "" && CustomerDataGrid != null
            //   ? _customerHandler.GetCustomers(textBox.Text)
            //   :_customerHandler.GetCustomers();
            //CustomerDataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }

        private void CustomerSearchTextBox_OnFocusChanged(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            textBox.Text =
                textBox.Text == "" ? _searchName : textBox.Text;
        }

        private void CustomerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCustomer = CustomerDataGrid.SelectedItem as Customer;
            if (SelectedCustomer == null) return;

            CustomerIdTextBox.Text = SelectedCustomer.Id.ToString();
            NameTextBox.Text = SelectedCustomer.Name;
            ContactInfoTextBox.Text = SelectedCustomer.Contact;
            NicTextBox.Text = SelectedCustomer.Nic;
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select the relevant customer");
                DialogResult = false;
            }
            else
            {
                // MessageBox.Show("Credit customer verified");
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}