using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Customers.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddServiceDialog.xaml
    /// </summary>
    public partial class AddServiceDialog : Window
    {
        private readonly ProductHandler _productHandler;

        public AddServiceDialog()
        {
            _productHandler = new ProductHandler();
            ServiceSource = _productHandler.GetProducts(productType: ProductType.Service).Cast<Service>();
            InitializeComponent();
            ServiceNameComboBox.Focus();
        }

        public IEnumerable<Service> ServiceSource { get; set; }
        public Service SelectedService { get; set; }
        public decimal ServiceCharge { get; set; }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Validation

                if (ServiceChargeTextBox.Text == "" || ServiceNameComboBox.Text == "")
                {
                    MessageBox.Show("Empty Inputs.", "Invalid Inputs");
                    return;
                }

                if (decimal.Parse(ServiceChargeTextBox.Text) <= 0)
                {
                    MessageBox.Show("Please Enter Valid Service Charge");
                    ServiceChargeTextBox.Text = "";
                    return;
                }

                #endregion

                var serviceName = ServiceNameComboBox.Text;
                ServiceCharge = decimal.Parse(ServiceChargeTextBox.Text);

                // Search in database for current service name and check for any existing records
                var searchResults = ServiceSource.Where(s => s.Name == serviceName).ToArray();
                Debug.WriteLine("Search Parameter: " + serviceName + ". Results count: " + searchResults.Length);

                //if service IS there in DB,
                if (searchResults.Length == 1)
                {
                    SelectedService = searchResults.Single();
                }
                else
                {
                    var service = new Service(serviceName);
                    _productHandler.AddProduct(service);
                    SelectedService = service;
                }

                DialogResult = true;
            }

            catch (FormatException)
            {
                MessageBox.Show("Please enter valid service charge");
                ServiceChargeTextBox.Text = "";
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}