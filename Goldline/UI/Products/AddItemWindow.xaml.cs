using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Products
{
    /// <summary>
    ///     Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        private readonly ProductHandler _productHandler;
        private readonly ProductType _ProductType;
        private readonly ProductHandler.AlloywheelHandler alloywheelHandler;
        private readonly ProductHandler.BatteryHandler batteryHandler;
        private readonly ProductHandler.TyreHandler tyreHandler;

        public AddItemWindow(ProductType ProductType)
        {
            _ProductType = ProductType;
            _productHandler = new ProductHandler();
            LoadPropertySources();
            Item = InitializeItem();

            InitializeComponent();
            RefreshComboBoxes();

            // If ProductType is alloywheel, hide property2 row contents
            var rowItemVisibility = _ProductType == ProductType.Alloywheel ? Visibility.Hidden : Visibility.Visible;
            Property2Label.Visibility = rowItemVisibility;
            Property2ComboBox.Visibility = rowItemVisibility;

            AddProperty2Button.Visibility = _ProductType == ProductType.Alloywheel || _ProductType == ProductType.Tyre
                ? Visibility.Hidden
                : Visibility.Visible;
        }

        public Item Item { get; set; }
        public IEnumerable<string> BrandSource { get; set; }
        public IEnumerable<string> Property1Source { get; set; }
        public IEnumerable<string> Property2Source { get; set; }

        /// <summary>
        ///     Load new sources when initializing the window, and update combo boxes
        /// </summary>
        private void LoadPropertySources()
        {
            switch (_ProductType)
            {
                case ProductType.Alloywheel:
                    BrandSource = alloywheelHandler.GetBrands();
                    Property1Source = alloywheelHandler.GetDimensions();
                    Property2Source = null;
                    break;
                case ProductType.Battery:
                    BrandSource = batteryHandler.GetBrands();
                    Property1Source = null;
                    Property2Source = null;
                    break;
                case ProductType.Tyre:
                    BrandSource = tyreHandler.GetBrands();
                    Property1Source = tyreHandler.GetDimensions();
                    Property2Source = App.GetAllCountries();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RefreshComboBoxes();
        }

        private void RefreshComboBoxes()
        {
            BrandComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            Property1ComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            Property2ComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }

        /// <summary>
        ///     Finalize item by setting the relevant properties into item
        /// </summary>
        /// <returns></returns>
        private void UpdateItemProperties()
        {
            Item.Brand = (string) BrandComboBox.SelectedItem;
            Item.Model = ModelTextBox.Text;

            switch (_ProductType)
            {
                case ProductType.Alloywheel:
                    ((Alloywheel) Item).Dimension = (string) Property1ComboBox.SelectedItem;
                    break;
                case ProductType.Battery:
                    ((Battery) Item).Capacity = (string) Property1ComboBox.SelectedItem;
                    ((Battery) Item).Voltage = (string) Property2ComboBox.SelectedItem;
                    break;
                case ProductType.Tyre:
                    ((Tyre) Item).Dimension = (string) Property1ComboBox.SelectedItem;
                    ((Tyre) Item).Country = (string) Property2ComboBox.SelectedItem;
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        ///     Initialize an item of specified type when creating window
        /// </summary>
        /// <returns></returns>
        private Item InitializeItem()
        {
            switch (_ProductType)
            {
                case ProductType.Tyre:
                    return new Tyre();
                case ProductType.Alloywheel:
                    return new Alloywheel();
                case ProductType.Battery:
                    return new Battery();
                default:
                    return null;
            }
        }

        private bool IsDataInCorrectForm()
        {
            return
                !(ItemCodeTextBox.Text == "" || (string) BrandComboBox.SelectedItem == "" ||
                  (string) Property1ComboBox.SelectedItem == "" ||
                  _ProductType != ProductType.Alloywheel && (string) Property2ComboBox.SelectedItem == "" ||
                  StockTextBox.Text == "" || PriceTextBox.Text == "");
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsDataInCorrectForm())
            {
                UpdateItemProperties();
                _productHandler.AddProduct(Item);
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

        private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateItemProperties();
            NameTextBox.Text = Item.ToString();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateItemProperties();
            NameTextBox.Text = Item.ToString();
        }

        #region Binding Properties

        public string Property1Text
        {
            get
            {
                switch (_ProductType)
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
                switch (_ProductType)
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

        #endregion

        #region Adding new properties

        private void AddNewBrand(string brand)
        {
            switch (_ProductType)
            {
                case ProductType.Alloywheel:
                    alloywheelHandler.AddBrand(brand);
                    break;
                case ProductType.Battery:
                    batteryHandler.AddBrand(brand);
                    break;
                case ProductType.Tyre:
                    tyreHandler.AddBrand(brand);
                    break;
                default:
                    return;
            }
            LoadPropertySources();
        }

        private void AddNewProperty1(string property)
        {
            switch (_ProductType)
            {
                case ProductType.Alloywheel:
                    alloywheelHandler.AddDimension(property);
                    break;
                case ProductType.Tyre:
                    alloywheelHandler.AddDimension(property);
                    break;
                default:
                    return;
            }
            LoadPropertySources();
        }

        private void AddNewProperty2(string property)
        {
            LoadPropertySources();
        }

        #endregion

        #region ButtonClicks for adding new properties

        private void AddBrandButton_OnClick(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog;
            switch (_ProductType)
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

            AddNewBrand(inputDialog.Response);
            LoadPropertySources();
        }

        private void AddProperty1Button_OnClick(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog;
            switch (_ProductType)
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

            AddNewProperty1(inputDialog.Response);
            LoadPropertySources();
        }

        private void AddProperty2Button_OnClick(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog;
            switch (_ProductType)
            {
                case ProductType.Battery:
                    inputDialog = new InputDialog("Add Voltage", "Enter Battery Voltage");
                    inputDialog.ShowDialog();
                    break;
                default:
                    return;
            }
            if (inputDialog.DialogResult != true) return;

            AddNewProperty2(inputDialog.Response);
            LoadPropertySources();
        }

        #endregion
    }
}