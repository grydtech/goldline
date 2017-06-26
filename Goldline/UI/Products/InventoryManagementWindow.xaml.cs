using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Products
{
    /// <summary>
    ///     Interaction logic for InventoryManagementWindow.xaml
    /// </summary>
    public partial class InventoryManagementWindow : Window
    {
        private readonly ProductHandler.AlloywheelHandler _alloywheelHandler;
        private readonly ProductHandler.BatteryHandler _batteryHandler;
        private readonly ProductHandler _productHandler;
        private readonly ProductHandler.TyreHandler _tyreHandler;

        public InventoryManagementWindow()
        {
            _productHandler = new ProductHandler();
            _alloywheelHandler = new ProductHandler.AlloywheelHandler();
            _batteryHandler = new ProductHandler.BatteryHandler();
            _tyreHandler = new ProductHandler.TyreHandler();
            ItemSource = _productHandler.GetItems();
            ItemTypeSource = Enum.GetNames(typeof(ProductType)).ToList().GetRange(0, 3);
            LoadAllSources();

            InitializeComponent();
            UpdateControlVisibility();
        }

        public IEnumerable<Item> ItemSource { get; set; }
        public IEnumerable<string> ItemTypeSource { get; set; }

        #region MyRegion

        private bool UpdateAllItems()
        {
            if (!IsDataInCorrectForm()) return false;
            foreach (var item in ItemSource)
            {
                if (item is Alloywheel)
                    _alloywheelHandler.Update((Alloywheel) item);
                if (item is Battery)
                    _batteryHandler.Update((Battery) item);
                if (item is Tyre)
                    _tyreHandler.Update((Tyre) item);
            }
            return true;
        }

        #endregion

        /// <summary>
        ///     Updates the source variables from database
        /// </summary>
        private void LoadAllSources()
        {
            _tyreBrandSource = _tyreHandler.GetBrands();
            _tyreDimensionSource = _tyreHandler.GetDimensions();
            _tyreCountrySource = _tyreHandler.GetCountries();

            _alloywheelBrandSource = _alloywheelHandler.GetBrands();
            _alloywheelDimensionSource = _alloywheelHandler.GetDimensions();

            _batteryBrandSource = _batteryHandler.GetBrands();
            _batteryCapacitySource = null;
            _batteryVoltageSource = null;
        }

        #region ItemProperty TextBoxes Text

        private bool IsDataInCorrectForm()
        {
            return BrandComboBox.Text != "" && PriceTextBox.Text != "" &&
                   PriceTextBox.Text != "" && StockTextBox.Text != "" && Property1ComboBox.Text != "" &&
                   (Property2ComboBox.Text != "" || (ProductType) ItemTypeComboBox.SelectedIndex ==
                    ProductType.Alloywheel);
        }

        #endregion

        private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NameTextBox.Text = ((Item) InventoryDataGrid.SelectedItem)?.ToString();
        }

        private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = ((Item) InventoryDataGrid.SelectedItem)?.ToString();
        }

        #region Encapsulated data sources

        // variables storing all dimensions, brands, capacities, voltages and countries
        private IEnumerable<string> _tyreBrandSource;

        private IEnumerable<string> _tyreDimensionSource;
        private IEnumerable<string> _tyreCountrySource;

        private IEnumerable<string> _alloywheelBrandSource;
        private IEnumerable<string> _alloywheelDimensionSource;

        private IEnumerable<string> _batteryBrandSource;
        private IEnumerable<string> _batteryCapacitySource;
        private IEnumerable<string> _batteryVoltageSource;

        #endregion

        #region Binding Properties

        // Properties to bind to different values depending on ComboBox selection
        public string Property1Text
        {
            get
            {
                switch ((ProductType) ItemTypeComboBox.SelectedIndex)
                {
                    case ProductType.Alloywheel:
                        return nameof(Alloywheel.Dimension);
                    case ProductType.Battery:
                        return nameof(Battery.Capacity);
                    case ProductType.Tyre:
                        return nameof(Tyre.Dimension);
                    default:
                        return null;
                }
            }
        }

        public string Property2Text
        {
            get
            {
                switch ((ProductType) ItemTypeComboBox.SelectedIndex)
                {
                    case ProductType.Alloywheel:
                        return null;
                    case ProductType.Battery:
                        return nameof(Battery.Voltage);
                    case ProductType.Tyre:
                        return nameof(Tyre.Country);
                    default:
                        return null;
                }
            }
        }

        public IEnumerable<string> BrandSource
        {
            get
            {
                switch ((ProductType) ItemTypeComboBox.SelectedIndex)
                {
                    case ProductType.Alloywheel:
                        return _alloywheelBrandSource;
                    case ProductType.Battery:
                        return _batteryBrandSource;
                    case ProductType.Tyre:
                        return _tyreBrandSource;
                    default:
                        return null;
                }
            }
        }

        public IEnumerable<string> Property1Source
        {
            get
            {
                switch ((ProductType) ItemTypeComboBox.SelectedIndex)
                {
                    case ProductType.Alloywheel:
                        return _alloywheelDimensionSource;
                    case ProductType.Battery:
                        return _batteryCapacitySource;
                    case ProductType.Tyre:
                        return _tyreDimensionSource;
                    default:
                        return null;
                }
            }
        }

        public IEnumerable<string> Property2Source
        {
            get
            {
                switch ((ProductType) ItemTypeComboBox.SelectedIndex)
                {
                    case ProductType.Alloywheel:
                        return null;
                    case ProductType.Battery:
                        return _batteryVoltageSource;
                    case ProductType.Tyre:
                        return _tyreCountrySource;
                    default:
                        return null;
                }
            }
        }

        #endregion

        #region SearchBar(TextBox and ComboBox and KeyPress)

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (InventoryDataGrid.SelectedIndex < InventoryDataGrid.Items.Count - 1)
                        InventoryDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (InventoryDataGrid.SelectedIndex > 0) InventoryDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        ///     Update label names and refresh bindings when item type is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshComboBoxes();
            RefreshDataGrid();
        }

        #endregion

        #region Refresh Data Components

        private void RefreshDataGrid()
        {
            ItemSource = _productHandler.GetItems(SearchTextBox?.Text, (ProductType?) ItemTypeComboBox?.SelectedIndex);
            InventoryDataGrid?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }

        private void RefreshComboBoxes()
        {
            Property1Label?.GetBindingExpression(ContentProperty)?.UpdateTarget();
            Property2Label?.GetBindingExpression(ContentProperty)?.UpdateTarget();

            BrandComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            Property1ComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            Property2ComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            UpdateControlVisibility();
        }

        private void UpdateControlVisibility()
        {
            var rowVisibility = (ProductType) ItemTypeComboBox.SelectedIndex == ProductType.Alloywheel
                ? Visibility.Hidden
                : Visibility.Visible;
            if (Property2ComboBox != null) Property2ComboBox.Visibility = rowVisibility;
            if (Property2Label != null) Property2Label.Visibility = rowVisibility;
        }

        #endregion

        #region Buttons Click

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (Item) InventoryDataGrid.SelectedItem;
            var model = ModelTextBox.Text;
            var stockqty = uint.Parse(StockTextBox.Text);
            var unitPrice = decimal.Parse(PriceTextBox.Text);

            var brand = BrandComboBox.Text;
            var prop1 = Property1ComboBox.Text;
            var prop2 = Property2ComboBox.Text;

            if (IsDataInCorrectForm())
            {
                try
                {
                    switch ((ProductType)ItemTypeComboBox.SelectedIndex)
                    {
                        case ProductType.Alloywheel:
                            var alloywheel = (Alloywheel) item;
                            _alloywheelHandler.Update(
                                alloywheel,
                                model == alloywheel.Model ? null : model,
                                stockqty == alloywheel.StockQty ? null : (uint?) stockqty,
                                unitPrice == alloywheel.UnitPrice ? null : (decimal?) unitPrice,
                                brand == alloywheel.Brand ? null : brand,
                                prop1 == alloywheel.Dimension ? null : prop1);
                            break;
                        case ProductType.Battery:
                            var battery = (Battery) item;
                            _batteryHandler.Update(
                                battery,
                                model == battery.Model ? null : model,
                                stockqty == battery.StockQty ? null : (uint?) stockqty,
                                unitPrice == battery.UnitPrice ? null : (decimal?) unitPrice,
                                brand == battery.Brand ? null : brand,
                                prop1 == battery.Capacity ? null : prop1,
                                prop2 == battery.Voltage ? null : prop2);
                            break;
                        case ProductType.Tyre:
                            var tyre = (Tyre) item;
                            _tyreHandler.Update(
                                tyre,
                                model == tyre.Model ? null : model,
                                stockqty == tyre.StockQty ? null : (uint?) stockqty,
                                unitPrice == tyre.UnitPrice ? null : (decimal?) unitPrice,
                                brand == tyre.Brand ? null : brand,
                                prop1 == tyre.Dimension ? null : prop1,
                                prop2 == tyre.Country ? null : prop2);
                            break;
                        case ProductType.Service:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    MessageBox.Show("Successfully Updated", "Information", MessageBoxButton.OK);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Some parameters are invalid", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            SearchTextBox.Clear();
            LoadAllSources();
            RefreshDataGrid();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var addItemWindow = new AddItemWindow((ProductType) ItemTypeComboBox.SelectedIndex);
            addItemWindow.ShowDialog();
            LoadAllSources();
            RefreshComboBoxes();
            RefreshDataGrid();
        }

        #endregion
    }
}