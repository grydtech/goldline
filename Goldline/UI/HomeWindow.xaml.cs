using System;
using System.Windows;
using System.Windows.Threading;
using Core.Domain.Model;
using Core.Domain.Model.Employees;
using Goldline.UI.Customers;
using Goldline.UI.Employees;
using Goldline.UI.Products;
using Goldline.UI.Returns;
using Goldline.UI.Security;
using Goldline.UI.Suppliers;

namespace Goldline.UI
{
    /// <summary>
    ///     Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
            NameLabel.Content = Session.CurrentUser.Username;

            // Enable options depending on user access restrictions

            // Create timer object
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.1)};
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Content = DateTime.Now.ToLongTimeString();
        }


        /*
         * Button Operations
         */

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to logout?", "Confirm", MessageBoxButton.YesNo) !=
                MessageBoxResult.Yes) return;

            new LoginWindow().Show();
            Close();
        }

        private void ChangePasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
            new ChangePasswordWindow().ShowDialog();
        }

        private void ManageInventoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            new InventoryManagementWindow().Show();
        }

        private void ShowCatalogButton_OnClick(object sender, RoutedEventArgs e)
        {
            new CatalogWindow().Show();
        }

        private void ManageSuppliersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new SupplierManagementWindow().Show();
        }

        private void NewSupplyOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            new AddSupplierOrderWindow().Show();
        }

        private void SupplierOrdersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new SupplierOrdersWindow().Show();
        }

        private void NewCustomerOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddCustomerOrderWindow.GetAddCustomerOrderWindow().Show();
        }

        private void ItemReturnsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new ItemReturnManagementWindow().Show();
        }

        private void AddItemReturnsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new AddItemReturnWindow().Show();
        }

        private void ManageCustomersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new CustomerManagementWindow().Show();
        }

        private void CustomerPaymentsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new CustomerPaymentWindow().Show();
        }

        private void ManageEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeManagementWindow().Show();
        }

        private void EmployeePaymentsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new EmployeePaymentWindow().Show();
        }

        private void GetReportsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new TransactionReportWindow().Show();
        }

        private void ActivityLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet");
        }

        private void CustomerOrdersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new CustomerOrderDetailsWindow().Show();
        }
    }
}