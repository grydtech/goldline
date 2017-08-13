using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Core.Domain.Model.Inventory;

//using log4net;

namespace Goldline.UI.Customers.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddOrderDialog.xaml
    /// </summary>
    public partial class AddOrderDialog
    {
        private static AddOrderDialog _addOrderDialog;
        private readonly ProductHandler _productHandler;
        private decimal _discount;
        private decimal _unitPrice;

        private AddOrderDialog()
        {
            try
            {
                _productHandler = new ProductHandler();
                Order = new Order();
                ProductSource = _productHandler.GetItems();
                InitializeComponent();
                ProductTypeComboBox.ItemsSource = Enum.GetNames(typeof(ProductType));
                ProductTypeComboBox.Focus();
            }
            catch (Exception ex)
            {
                //   Log.Error(ex.Message);
                MessageBox.Show("Exception : " + ex.Message);
            }
        }

        public Order Order { get; set; }

        public IEnumerable<Product> ProductSource { get; set; }


        public static AddOrderDialog GetAddCustomerOrderWindow()
        {
            if (_addOrderDialog == null || !_addOrderDialog.IsLoaded)
                _addOrderDialog = new AddOrderDialog();
            return _addOrderDialog;
        }


        private void CalculateUnitPrice()
        {
            var selectedItem = ProductComboBox.SelectedItem as Item;
            if (selectedItem == null) return;
            var actualUnitPrice = selectedItem.UnitPrice;

            try
            {
                var discount = DiscountTextBox.Text == "" ? 0 : decimal.Parse(DiscountTextBox.Text);
                discount = Math.Round(discount, 2);
                var unitPriceTextBoxValue = UnitPriceTextBox.Text == "" ? 0 : decimal.Parse(UnitPriceTextBox.Text);

                if (discount < 100 && discount > -11)
                {
                    _discount = discount;
                    var unitPrice = actualUnitPrice * (100 - _discount) / 100;
                    unitPrice = Math.Round(unitPrice, 2);
                    if (unitPrice == unitPriceTextBoxValue) return;

                    _unitPrice = unitPrice;
                    UnitPriceTextBox.Text = _unitPrice != 0 ? _unitPrice.ToString(CultureInfo.InvariantCulture) : "";
                }
                else
                {
                    UnitPriceTextBox.Text = actualUnitPrice.ToString(CultureInfo.InvariantCulture);
                    DiscountTextBox.Text = "0";
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please Enter Values In correct Format", "Invalid Input");
                DiscountTextBox.Text = "0";
                UnitPriceTextBox.Text = actualUnitPrice.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void CalculateDiscount()
        {
            var price = UnitPriceTextBox.Text == "" ? 0 : decimal.Parse(UnitPriceTextBox.Text);
            price = Math.Round(price, 2);

            var discountTextBoxValue = DiscountTextBox.Text == "" ? 0 : decimal.Parse(DiscountTextBox.Text);
            discountTextBoxValue = Math.Round(discountTextBoxValue, 2);

            var selectedItem = ProductComboBox.SelectedItem as Item;
            if (selectedItem == null) return;
            if (price > 0)
            {
                Debug.Write(price);

                _unitPrice = price;
                var actualUnitPrice = selectedItem.UnitPrice;
                var discount = (actualUnitPrice - _unitPrice) * 100 / actualUnitPrice;
                discount = Math.Round(discount, 2);
                if (discount == discountTextBoxValue) return;
                if (discount >= -10)
                {
                    _discount = discount;
                    DiscountTextBox.Text = _discount != 0 ? _discount.ToString(CultureInfo.InvariantCulture) : "";
                }
                if (discount < -10)
                {
                    UnitPriceTextBox.Text = actualUnitPrice.ToString(CultureInfo.InvariantCulture);
                    DiscountTextBox.Text = "0";
                }
            }
            else
            {
                UnitPriceTextBox.Text = "";
            }
        }

        public bool IsAlreadyEntered(uint? id)
        {
            return Order.OrderItems.Any(orderEntry => orderEntry.ProductId == id);
        }

        private void UpdateGrandTotalLabel()
        {
            GrandTotalValueLabel?.GetBindingExpression(ContentProperty)?.UpdateTarget();
        }

        private void RefreshSearchComboBox()
        {
            // Update Data Grid with new set of products
            ProductSource = ProductTypeComboBox.SelectedItem != null && ProductComboBox != null
                ? _productHandler.GetProducts(productType: (ProductType) ProductTypeComboBox.SelectedIndex)
                : _productHandler.GetItems();
            ProductComboBox?.GetBindingExpression(ProductComboBox.ItemsSourceProperty)?.UpdateTarget();
        }

        public void RefreshOrderItemsDataGrid()
        {
            OrderItemsDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            OrderItemsDataGrid?.Items.Refresh();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Exception Handling

                if (QuantityTextBox.Text == "" || DiscountTextBox.Text == "" || UnitPriceTextBox.Text == "")
                {
                    MessageBox.Show("Some inputs are empty");
                    // Log.Debug("Quantity TextBox or Discount TextBox is Empty");
                    return;
                }
                else if (int.Parse(QuantityTextBox.Text) <= 0)
                {
                    //   Log.Debug("Qauntity is less than or equal to zero");
                    MessageBox.Show("Quantity is not valid");
                    return;
                }
                /* ALLOWED DISCOUNT TO BE NEGATIVE  */
                //else if (decimal.Parse(DiscountTextBox.Text) < 0)
                //{
                //    //   Log.Debug("Discount is negative ");
                //    MessageBox.Show("Discount is negative");
                //    return;
                //}
                else if (decimal.Parse(DiscountTextBox.Text) >= 100)
                {
                    //   Log.Debug("Discount greater than 100");
                    MessageBox.Show("Discount is more than 100%");
                    return;
                }
                else if (decimal.Parse(UnitPriceTextBox.Text) < 0)
                {
                    //   Log.Debug("Discount greater than 100");
                    MessageBox.Show("Unit Price is negative");
                    return;
                }

                #endregion

                var quantity = QuantityTextBox.Text == "" ? 0 : uint.Parse(QuantityTextBox.Text);
                var product = ProductComboBox.SelectedItem as Product;
                var item = product as Item;
                _discount = DiscountTextBox.Text == "" ? 0 : decimal.Parse(DiscountTextBox.Text);
                _unitPrice = UnitPriceTextBox.Text == "" ? 0 : decimal.Parse(UnitPriceTextBox.Text);

                if (product != null)
                {
                    #region validation

                    if (quantity > item?.StockQty)
                    {
                        MessageBox.Show("Not enough items in stock to fullfil your requirement", "Not Enough Stock");
                        //    Log.Debug("Entered quantity is greater than available items");
                        return;
                    }
                    if (IsAlreadyEntered(product.Id))
                    {
                        // Log.Debug("Attempted to enter same item twice");
                        MessageBox.Show("This entry is already entered once. Try updating its quantity instead");
                    }

                    #endregion

                    else
                    {
                        var salePrice = _unitPrice != 0 ? _unitPrice : item?.UnitPrice * (100 - _discount) / 100 ?? 0;
                        var orderItem = new OrderItem(product.Id.GetValueOrDefault(), product.Name, quantity,
                            salePrice);

                        // add items to the order entries list
                        Order.AddOrderItem(orderItem);
                        UpdateGrandTotalLabel();
                        RefreshSearchComboBox();
                        RefreshOrderItemsDataGrid();

                        ProductTypeComboBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Product!", "Product not selected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input");
            }
            finally
            {
                QuantityTextBox.Text = "";
                UnitPriceTextBox.Text = "";
                DiscountTextBox.Text = "";
                ProductComboBox.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Order.OrderItems.Count == 0)
            {
                MessageBox.Show("Add products to proceed!", "Empty order");
                return;
            }
            try
            {
                var window = new OrderCheckoutDialog(Order);
                window.ShowDialog();
                if (window.DialogResult != true) return;

                Order = new Order();
                RefreshOrderItemsDataGrid();
                UpdateGrandTotalLabel();
                ProductSource = _productHandler.GetItems();
                ProductComboBox.GetBindingExpression(ProductComboBox.ItemsSourceProperty)?.UpdateTarget();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "   :  An error has occured!");
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEntry = OrderItemsDataGrid.SelectedItem as OrderItem;
            if (selectedEntry != null)
            {
                Order.RemoveOrderItem(selectedEntry);
                UpdateGrandTotalLabel();
                RefreshOrderItemsDataGrid();
            }
            else
            {
                MessageBox.Show("Please select an item to remove");
            }
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox) sender).SelectAll();
        }

        private void SearchComboBox_OnSelectionConfirmed(object sender, RoutedEventArgs e)
        {
            try
            {
                var product = ProductComboBox.SelectedItem as Product;
                var item = product as Item;
                if (product != null)
                {
                    QuantityTextBox.Text = "1";
                    UnitPriceTextBox.Text = item?.UnitPrice.ToString(CultureInfo.InvariantCulture);
                    DiscountTextBox.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
        }

        //private void DiscountTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    CalculateUnitPrice();
        //}

        //private void UnitPriceTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    CalculateDiscount();
        //}

        #region Window Event Handling

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSearchComboBox();
            QuantityTextBox.Text = "";
            UnitPriceTextBox.Text = "";
            DiscountTextBox.Text = "";
        }

        private void DiscountTextBox_FocusChanged(object sender, RoutedEventArgs e)
        {
            CalculateUnitPrice();
        }

        private void UnitPriceTextBox_FocusChanged(object sender, RoutedEventArgs e)
        {
            CalculateDiscount();
        }

        private void UnitPriceTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CalculateDiscount();
        }

        private void DiscountTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CalculateUnitPrice();
        }

        #endregion
    }
}