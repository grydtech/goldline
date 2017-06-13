using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Model.Enums;
using Core.Model.Handlers;
using Core.Model.Products;

namespace Goldline.UI.Products
{
    /// <summary>
    ///     Interaction logic for InventoryManagementWindow.xaml
    /// </summary>
    public partial class InventoryManagementWindow : Window
    {
        private readonly ProductDetailHandler _productDetailHandler;
        private readonly ProductHandler _productHandler;

        public InventoryManagementWindow()
        {
            _productHandler = new ProductHandler();
            _productDetailHandler = new ProductDetailHandler();
            ItemSource = _productHandler.GetItems("");
            ItemTypeSource = Enum.GetNames(typeof(ItemType));
            LoadAllSources();

            InitializeComponent();
            UpdateControlVisibility();
        }

        public IEnumerable<Item> ItemSource { get; set; }
        public IEnumerable<string> ItemTypeSource { get; set; }

        private bool UpdateAllItems()
        {
            if (!IsDataInCorrectForm()) return false;
            foreach (var item in ItemSource)
            {
                _productHandler.UpdateProduct(item);
            }
            return true;
        }

        /// <summary>
        ///     Updates the source variables from database
        /// </summary>
        private void LoadAllSources()
        {
            _tyreBrandSource = _productDetailHandler.GetAllTyreBrands();
            _tyreDimensionSource = _productDetailHandler.GetAllTyreDimensions();
            _tyreCountrySource = App.GetAllCountries();

            _alloywheelBrandSource = _productDetailHandler.GetAllAlloywheelBrands();
            _alloywheelDimensionSource = _productDetailHandler.GetAllAlloywheelDimensions();

            _batteryBrandSource = _productDetailHandler.GetAllBatteryBrands();
            _batteryCapacitySource = _productDetailHandler.GetAllBatteryCapacities();
            _batteryVoltageSource = _productDetailHandler.GetAllBatteryVoltages();
        }

        #region ItemProperty TextBoxes Text

        private bool IsDataInCorrectForm()
        {
            return ItemCodeTextBox.Text != "" && BrandComboBox.Text != "" && PriceTextBox.Text != "" &&
                   PriceTextBox.Text != "" && StockTextBox.Text != "" && Property1ComboBox.Text != "" &&
                   (Property2ComboBox.Text != "" || (ItemType) ItemTypeComboBox.SelectedIndex == ItemType.Alloywheel);
        }

        #endregion

        private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NameTextBox.Text = ((Item) InventoryDataGrid.SelectedItem)?.GenerateName();
        }

        private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = ((Item) InventoryDataGrid.SelectedItem)?.GenerateName();
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
                switch ((ItemType) ItemTypeComboBox.SelectedIndex)
                {
                    case ItemType.Alloywheel:
                        return nameof(Alloywheel.Dimension);
                    case ItemType.Battery:
                        return nameof(Battery.Capacity);
                    case ItemType.Tyre:
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
                switch ((ItemType) ItemTypeComboBox.SelectedIndex)
                {
                    case ItemType.Alloywheel:
                        return null;
                    case ItemType.Battery:
                        return nameof(Battery.Voltage);
                    case ItemType.Tyre:
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
                switch ((ItemType) ItemTypeComboBox.SelectedIndex)
                {
                    case ItemType.Alloywheel:
                        return _alloywheelBrandSource;
                    case ItemType.Battery:
                        return _batteryBrandSource;
                    case ItemType.Tyre:
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
                switch ((ItemType) ItemTypeComboBox.SelectedIndex)
                {
                    case ItemType.Alloywheel:
                        return _alloywheelDimensionSource;
                    case ItemType.Battery:
                        return _batteryCapacitySource;
                    case ItemType.Tyre:
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
                switch ((ItemType) ItemTypeComboBox.SelectedIndex)
                {
                    case ItemType.Alloywheel:
                        return null;
                    case ItemType.Battery:
                        return _batteryVoltageSource;
                    case ItemType.Tyre:
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
            ItemSource = _productHandler.GetItems(SearchTextBox?.Text, (ItemType?) ItemTypeComboBox?.SelectedIndex);
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
            var rowVisibility = (ItemType) ItemTypeComboBox.SelectedIndex == ItemType.Alloywheel
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
            if (IsDataInCorrectForm())
            {
                if (UpdateAllItems())
                {
                    MessageBox.Show("Successfully Updated", "Information", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Duplicate Entry", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Check The Entered Data!!", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            SearchTextBox.Text = "";
            LoadAllSources();
            RefreshDataGrid();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var addItemWindow = new AddItemWindow((ItemType) ItemTypeComboBox.SelectedIndex);
            addItemWindow.ShowDialog();
            LoadAllSources();
            RefreshComboBoxes();
            RefreshDataGrid();
        }

        #endregion
    }
}