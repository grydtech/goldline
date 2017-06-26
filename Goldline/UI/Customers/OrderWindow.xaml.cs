using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Core.Domain.Model.Inventory;
using Goldline.UI.Invoices;

//using log4net;

namespace Goldline.UI.Customers
{
    /// <summary>
    ///     Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private static OrderWindow _orderWindow;
        private readonly OrderHandler _orderHandler;
        private readonly ProductHandler _productHandler;
        private decimal _discount;
        private decimal _unitPrice;

        private OrderWindow()
        {
            try
            {
                _productHandler = new ProductHandler();
                _orderHandler = new OrderHandler();
                Order = new Order();
                ItemSource = _productHandler.GetItems(Uid);
                InitializeComponent();
                ComboBox.ItemsSource = Enum.GetNames(typeof(ProductType)).ToList().GetRange(0, 3);
                ComboBox.Focus();
            }
            catch (Exception ex)
            {
                //   Log.Error(ex.Message);
                MessageBox.Show("Exception : " + ex.Message);
            }
        }

        //private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Order Order { get; set; }

        public IEnumerable<Item> ItemSource { get; set; }


        public static OrderWindow GetAddCustomerOrderWindow()
        {
            if (_orderWindow == null || !_orderWindow.IsLoaded)
                _orderWindow = new OrderWindow();
            return _orderWindow;
        }


        private void CalculateUnitPrice()
        {
            var discount = DiscountTextBox.Text == "" ? 0 : decimal.Parse(DiscountTextBox.Text);
            discount = Math.Round(discount, 2);

            var unitPriceTextBoxValue = UnitPriceTextBox.Text == "" ? 0 : decimal.Parse(UnitPriceTextBox.Text);

            var selectedItem = SearchDataGrid.SelectedItem as Item;
            if (selectedItem == null) return;

            var actualUnitPrice = selectedItem.UnitPrice;
            if (discount <= 100)
            {
                _discount = discount;
                var unitPrice = actualUnitPrice * (100 - _discount) / 100;
                unitPrice = Math.Round(unitPrice, 2);
                if (unitPrice == unitPriceTextBoxValue) return;

                _unitPrice = unitPrice;
                UnitPriceTextBox.Text = _unitPrice != 0 ? _unitPrice.ToString() : "";
            }
            else
            {
                UnitPriceTextBox.Text = actualUnitPrice.ToString();
            }
        }

        private void CalculateDiscount()
        {
            var price = UnitPriceTextBox.Text == "" ? 0 : decimal.Parse(UnitPriceTextBox.Text);
            price = Math.Round(price, 2);

            var discountTextBoxValue = DiscountTextBox.Text == "" ? 0 : decimal.Parse(DiscountTextBox.Text);
            discountTextBoxValue = Math.Round(discountTextBoxValue, 2);

            var selectedItem = SearchDataGrid.SelectedItem as Item;
            if (selectedItem == null) return;

            if (price > 0)
            {
                _unitPrice = price;
                var actualUnitPrice = selectedItem.UnitPrice;
                var discount = (actualUnitPrice - _unitPrice) * 100 / actualUnitPrice;
                discount = Math.Round(discount, 2);
                if (discount == discountTextBoxValue) return;

                _discount = discount;
                DiscountTextBox.Text = _discount != 0 ? _discount.ToString() : "";
            }
            else
            {
                UnitPriceTextBox.Text = "";
            }
        }

        public void GenerateInvoice()
        {
            new OrderInvoice(Order).Show();
        }

        public bool IsAlreadyEntered(uint? id)
        {
            return Order.OrderItems.Any(orderEntry => orderEntry.ProductId == id);
        }

        private void UpdateGrandTotalLabel()
        {
            
            GrandTotalValueLabel?.GetBindingExpression(ContentProperty)?.UpdateTarget();
        }

        private void RefreshSearchDataGrid()
        {
            // Update Data Grid with new set of products
            ItemSource = ComboBox.SelectedItem != null && SearchDataGrid != null
                ? _productHandler.GetItems(SearchTextBox.Text, (ProductType) ComboBox.SelectedIndex)
                : _productHandler.GetItems(SearchTextBox.Text);
            SearchDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
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
                var selectedItem = SearchDataGrid.SelectedItem as Item;
                _discount = DiscountTextBox.Text == "" ? 0 : decimal.Parse(DiscountTextBox.Text);
                _unitPrice = UnitPriceTextBox.Text == "" ? 0 : decimal.Parse(UnitPriceTextBox.Text);

                if (selectedItem != null)
                {
                    #region validation

                    if (quantity > selectedItem.StockQty)
                    {
                        MessageBox.Show("Not enough items in stock to fullfil your requirement", "Not Enough Stock");
                        //    Log.Debug("Entered quantity is greater than available items");
                        return;
                    }
                    if (IsAlreadyEntered(selectedItem.Id))
                    {
                        // Log.Debug("Attempted to enter same item twice");
                        MessageBox.Show("This entry is already entered once. Try updating its quantity instead");
                    }

                    #endregion

                    else
                    {
                        var salePrice = _unitPrice != 0 ? _unitPrice : selectedItem.UnitPrice * (100 - _discount) / 100;
                        var orderItem = new OrderItem(selectedItem.Id.Value, selectedItem.Name, quantity, salePrice);

                        // add items to the order entries list
                        Order.AddOrderItem(orderItem);
                        UpdateGrandTotalLabel();
                        RefreshSearchDataGrid();
                        RefreshOrderItemsDataGrid();

                        ComboBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Please select an item!", "Item not selected");
                }
            }
            catch (Exception ex)
            {
                //   Log.Debug(ex.Message);
                MessageBox.Show(ex.Message, "Invalid Input");
            }
            finally
            {
                QuantityTextBox.Text = "";
                UnitPriceTextBox.Text = "";
                DiscountTextBox.Text = "";
                SearchDataGrid.Items.Refresh();
               // UnitPriceTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            }
        }
        
        private void CreditCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Order.OrderItems.Count == 0)
            {
                MessageBox.Show("Add products to proceed!", "Empty order");
                return;
            }
            try
            {
                Order.Note = NoteTextBox.Text;

                var window = new OrderCheckoutWindow();
                window.ShowDialog();

                if (window.DialogResult == true)
                {
                    // Mark order as credit order and assign customerId to it
                    Order.CustomerId = window.SelectedCustomer.Id;

                    //here AddOrder meth ssgould return bool .THEN only we can generate success msg below
                    _orderHandler.AddOrder(Order);
                    
                    
                    MessageBox.Show(
                        "Order added successfully. " +
                        "Order Type: Credit. " +
                        "Customer Name: " + window.SelectedCustomer.Name);
                }

                // Show and print the invoice option should come here
                GenerateInvoice();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "   :  An error has occured!");
            }
        }

        private void CashCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Note: update stocks handled internally using triggers so its not required here
            if (Order.OrderItems.Count == 0)
            {
                MessageBox.Show("Add products to proceed!", "Empty order");
                return;
            }

            try
            {
                Order.Note = NoteTextBox.Text;
                _orderHandler.AddOrder(Order);
                MessageBox.Show("Order added successfully. Order Type: Cash");

                GenerateInvoice();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " :   An error has occured!");
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
                //    Log.Debug("items not selected to remove");
                MessageBox.Show("Please select an item to remove");
            }
        }

        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new Dialogs.AddServiceDialog();
            window.ShowDialog();

            if (window.DialogResult == null || !window.DialogResult.Value) return;

            var selectedService = window.SelectedService;
            var serviceCharge = window.ServiceCharge;

            if (!IsAlreadyEntered(selectedService.Id))
            {
                Order.OrderItems.Add(new OrderItem(selectedService.Id.Value, selectedService.Name, 1, serviceCharge));
                UpdateGrandTotalLabel();
                RefreshOrderItemsDataGrid();
            }
            else
            {
                MessageBox.Show("Duplicate Entry in the order");
            }
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox) sender).SelectAll();
        }

        #region Window Event Handling

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSearchDataGrid();
            NameTextBox.Text = "";
            QuantityTextBox.Text = "";
            UnitPriceTextBox.Text = "";
            DiscountTextBox.Text = "";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshSearchDataGrid();
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (SearchDataGrid.SelectedIndex < SearchDataGrid.Items.Count - 1)
                    {
                        SearchDataGrid.SelectedIndex++;
                    }
                    break;
                case Key.Up:
                    if (SearchDataGrid.SelectedIndex > 0)
                    {
                        SearchDataGrid.SelectedIndex--;
                    }
                    break;
            }
        }

        #endregion

        private void SearchDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = SearchDataGrid.SelectedItem as Item;
                if (item != null)
                {
                    NameTextBox.Text = item.Name;
                    QuantityTextBox.Text = "1";
                    UnitPriceTextBox.Text = item.UnitPrice.ToString();
                    DiscountTextBox.Text = "0";
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
        }
    }
}