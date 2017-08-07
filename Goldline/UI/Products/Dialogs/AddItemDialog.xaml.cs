using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Products.Dialogs
{
    /// <summary>
    ///     Interaction logic for AddItemDialog.xaml
    /// </summary>
    public partial class AddItemDialog : Window
    {
        private readonly ProductHandler.AlloywheelHandler _alloywheelHandler;
        private readonly ProductHandler.BatteryHandler _batteryHandler;
        private readonly ProductHandler _productHandler;
        private readonly ProductType _productType;
        private readonly ProductHandler.TyreHandler _tyreHandler;

        public AddItemDialog(ProductType productType)
        {
            // Load handlers and initialize item
            _productType = productType;
            _productHandler = new ProductHandler();
            _alloywheelHandler = new ProductHandler.AlloywheelHandler();
            _batteryHandler = new ProductHandler.BatteryHandler();
            _tyreHandler = new ProductHandler.TyreHandler();
            LoadSources();
            InitializeComponent();
            InitializeUi();
        }

        private void InitializeUi()
        {
            switch (_productType)
            {
                case ProductType.Alloywheel:
                    RowCapacity.Height = new GridLength(0);
                    RowVoltage.Height = new GridLength(0);
                    RowCountry.Height = new GridLength(0);
                    break;
                case ProductType.Battery:
                    RowDimension.Height = new GridLength(0);
                    RowCountry.Height = new GridLength(0);
                    break;
                case ProductType.Tyre:
                    RowCapacity.Height = new GridLength(0);
                    RowVoltage.Height = new GridLength(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Load new sources when initializing the window, and update combo boxes
        /// </summary>
        private void LoadSources()
        {
            switch (_productType)
            {
                case ProductType.Alloywheel:
                    BrandSource = _alloywheelHandler.GetBrands();
                    DimensionSource = _alloywheelHandler.GetDimensions();
                    CountrySource = null;
                    break;
                case ProductType.Battery:
                    BrandSource = _batteryHandler.GetBrands();
                    DimensionSource = null;
                    CountrySource = null;
                    break;
                case ProductType.Tyre:
                    BrandSource = _tyreHandler.GetBrands();
                    DimensionSource = _tyreHandler.GetDimensions();
                    CountrySource = _tyreHandler.GetCountries();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RefreshComboBoxes()
        {
            BrandComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            DimensionComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            CountryComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            Item item;
            switch (_productType)
            {
                case ProductType.Alloywheel:
                    item = new Alloywheel(Brand, Model, Stocks, UnitPrice, Dimension);
                    break;
                case ProductType.Battery:
                    item = new Battery(Brand, Model, Stocks, UnitPrice, Capacity, Voltage);
                    break;
                case ProductType.Tyre:
                    item = new Tyre(Brand, Model, Stocks, UnitPrice, Dimension, Country);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (item.Validate())
            {
                _productHandler.AddProduct(item);
            }
            else
            {
                MessageBox.Show("Some required information are missing. Please check again.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        #region Binding Properties

        public IEnumerable<string> BrandSource { get; set; }
        public IEnumerable<string> DimensionSource { get; set; }
        public IEnumerable<string> CountrySource { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Dimension { get; set; }
        public string Country { get; set; }
        public string Capacity { get; set; }
        public string Voltage { get; set; }
        public uint Stocks { get; set; }
        public decimal UnitPrice { get; set; }

        #endregion

        #region Adding new properties

        private void AddBrand(string brand)
        {
            switch (_productType)
            {
                case ProductType.Alloywheel:
                    _alloywheelHandler.AddBrand(brand);
                    break;
                case ProductType.Battery:
                    _batteryHandler.AddBrand(brand);
                    break;
                case ProductType.Tyre:
                    _tyreHandler.AddBrand(brand);
                    break;
                default:
                    return;
            }
            LoadSources();
            RefreshComboBoxes();
        }

        private void AddDimension(string dimension)
        {
            switch (_productType)
            {
                case ProductType.Alloywheel:
                    _alloywheelHandler.AddDimension(dimension);
                    break;
                case ProductType.Tyre:
                    _tyreHandler.AddDimension(dimension);
                    break;
                default:
                    return;
            }
            LoadSources();
            RefreshComboBoxes();
        }

        private void AddCountry(string country)
        {
            _tyreHandler.AddCountry(country);
            LoadSources();
            RefreshComboBoxes();
        }

        #endregion

        #region ButtonClicks for adding new properties

        private void AddBrandButton_OnClick(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog;
            switch (_productType)
            {
                case ProductType.Alloywheel:
                    inputDialog = new InputDialog("Add Brand", "Enter Alloywheel Brand");
                    inputDialog.ShowDialog();
                    break;
                case ProductType.Battery:
                    inputDialog = new InputDialog("Add Brand", "Enter Battery Brand");
                    inputDialog.ShowDialog();
                    break;
                case ProductType.Tyre:
                    inputDialog = new InputDialog("Add Brand", "Enter Tyre Brand");
                    inputDialog.ShowDialog();
                    break;
                default:
                    return;
            }
            if (inputDialog.DialogResult != true) return;
            AddBrand(inputDialog.Response);
        }

        private void AddDimensionButton_OnClick(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog;
            switch (_productType)
            {
                case ProductType.Alloywheel:
                    inputDialog = new InputDialog("Add Dimension", "Enter Alloywheel Dimension");
                    inputDialog.ShowDialog();
                    break;
                case ProductType.Battery:
                    inputDialog = new InputDialog("Add Capacity", "Enter Battery Capacity");
                    inputDialog.ShowDialog();
                    break;
                case ProductType.Tyre:
                    inputDialog = new InputDialog("Add Dimension", "Enter Tyre Dimension");
                    inputDialog.ShowDialog();
                    break;
                default:
                    return;
            }
            if (inputDialog.DialogResult != true) return;
            AddDimension(inputDialog.Response);
        }

        private void AddCountryButton_OnClick(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog;
            switch (_productType)
            {
                case ProductType.Tyre:
                    inputDialog = new InputDialog("Add Country", "Enter Country Name");
                    inputDialog.ShowDialog();
                    break;
                default:
                    return;
            }
            if (inputDialog.DialogResult != true) return;
            AddCountry(inputDialog.Response);
        }

        #endregion
    }
}