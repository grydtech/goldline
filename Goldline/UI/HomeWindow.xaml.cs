using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Core.Domain.Model;
using Goldline.UI.Customers;
using Goldline.UI.Customers.Dialogs;
using Goldline.UI.Employees;
using Goldline.UI.Products;
using Goldline.UI.Returns;
using Goldline.UI.Returns.Dialogs;
using Goldline.UI.Security;
using Goldline.UI.Suppliers;
using Goldline.UI.Suppliers.Dialogs;

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
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLabel.Content = DateTime.Now.ToLongTimeString();
        }


        /*
         * Button Operations
         */

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangePasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
            new ChangePasswordDialog().ShowDialog();
        }

        private void ManageProductsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new ProductWindow().Show();
        }

        private void ShowCatalogButton_OnClick(object sender, RoutedEventArgs e)
        {
            new CatalogWindow().Show();
        }

        private void ManageSuppliersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new SupplierWindow().Show();
        }

        private void NewPurchaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            new AddPurchaseDialog().Show();
        }

        private void PurchaseHistoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            new PurchaseHistoryWindow().Show();
        }

        private void NewOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddOrderDialog.GetAddCustomerOrderWindow().Show();
        }

        private void ItemReturnHistoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            new ItemReturnManagementWindow().Show();
        }

        private void NewItemReturnButton_OnClick(object sender, RoutedEventArgs e)
        {
            new AddItemReturnWindow().Show();
        }

        private void ManageCustomersButton_OnClick(object sender, RoutedEventArgs e)
        {
            new CustomerWindow().Show();
        }

        private void OrderPaymentHistoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            new OrderHistoryWindow().Show();
        }

        private void ManageEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeWindow().Show();
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

        private void OrderHistoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            new OrderHistoryWindow().Show();
        }

        private void HomeWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to logout?", "Confirm", MessageBoxButton.YesNo) !=
                MessageBoxResult.Yes)
                e.Cancel = true;
            else
                new LoginWindow().Show();
        }
    }
}