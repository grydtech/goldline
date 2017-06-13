using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Core.Model.Enums;
using Core.Model.Handlers;
using Core.Model.Products;

namespace Goldline.UI.Products
{
    /// <summary>
    ///     Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        private readonly ItemType _itemType;
        private readonly ProductDetailHandler _productDetailHandler;
        private readonly ProductHandler _productHandler;

        public AddItemWindow(ItemType itemType)
        {
            _itemType = itemType;
            _productDetailHandler = new ProductDetailHandler();
            _productHandler = new ProductHandler();
            LoadPropertySources();
            Item = InitializeItem();

            InitializeComponent();
            RefreshComboBoxes();

            // If itemType is alloywheel, hide property2 row contents
            var rowItemVisibility = _itemType == ItemType.Alloywheel ? Visibility.Hidden : Visibility.Visible;
            Property2Label.Visibility = rowItemVisibility;
            Property2ComboBox.Visibility = rowItemVisibility;

            AddProperty2Button.Visibility = _itemType == ItemType.Alloywheel || _itemType == ItemType.Tyre
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
            switch (_itemType)
            {
                case ItemType.Alloywheel:
                    BrandSource = _productDetailHandler.GetAllAlloywheelBrands();
                    Property1Source = _productDetailHandler.GetAllAlloywheelDimensions();
                    Property2Source = null;
                    break;
                case ItemType.Battery:
                    BrandSource = _productDetailHandler.GetAllBatteryBrands();
                    Property1Source = _productDetailHandler.GetAllBatteryCapacities();
                    Property2Source = _productDetailHandler.GetAllBatteryVoltages();
                    break;
                case ItemType.Tyre:
                    BrandSource = _productDetailHandler.GetAllTyreBrands();
                    Property1Source = _productDetailHandler.GetAllTyreDimensions();
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
            Item.ItemCode = ItemCodeTextBox.Text;
            Item.Brand = (string) BrandComboBox.SelectedItem;
            Item.Model = ModelTextBox.Text;

            switch (_itemType)
            {
                case ItemType.Alloywheel:
                    ((Alloywheel) Item).Dimension = (string) Property1ComboBox.SelectedItem;
                    break;
                case ItemType.Battery:
                    ((Battery) Item).Capacity = (string) Property1ComboBox.SelectedItem;
                    ((Battery) Item).Voltage = (string) Property2ComboBox.SelectedItem;
                    break;
                case ItemType.Tyre:
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
            switch (_itemType)
            {
                case ItemType.Tyre:
                    return new Tyre();
                case ItemType.Alloywheel:
                    return new Alloywheel();
                case ItemType.Battery:
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
                  (_itemType != ItemType.Alloywheel && (string) Property2ComboBox.SelectedItem == "") ||
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
            NameTextBox.Text = Item.GenerateName();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateItemProperties();
            NameTextBox.Text = Item.GenerateName();
        }

        #region Binding Properties

        public string Property1Text
        {
            get
            {
                switch (_itemType)
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
                switch (_itemType)
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

        #endregion

        #region Adding new properties

        private void AddNewBrand(string brand)
        {
            switch (_itemType)
            {
                case ItemType.Alloywheel:
                    _productDetailHandler.AddNewAlloywheelBrand(brand);
                    break;
                case ItemType.Battery:
                    _productDetailHandler.AddNewBatteryBrand(brand);
                    break;
                case ItemType.Tyre:
                    _productDetailHandler.AddNewTyreBrand(brand);
                    break;
                default:
                    return;
            }
            LoadPropertySources();
        }

        private void AddNewProperty1(string property)
        {
            switch (_itemType)
            {
                case ItemType.Alloywheel:
                    _productDetailHandler.AddNewAlloywheelDimension(property);
                    break;
                case ItemType.Tyre:
                    _productDetailHandler.AddNewTyreDimension(property);
                    break;
                case ItemType.Battery:
                    _productDetailHandler.AddNewBatteryCapacity(uint.Parse(property));
                    break;
                default:
                    return;
            }
            LoadPropertySources();
        }

        private void AddNewProperty2(string property)
        {
            switch (_itemType)
            {
                case ItemType.Battery:
                    _productDetailHandler.AddNewBatteryVoltage(uint.Parse(property));
                    break;
                default:
                    return;
            }
            LoadPropertySources();
        }

        #endregion

        #region ButtonClicks for adding new properties

        private void AddBrandButton_OnClick(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog;
            switch (_itemType)
            {
                case ItemType.Alloywheel:
                    inputDialog = new InputDialog("Add Brand", "Enter Alloywheel Brand");
                    inputDialog.ShowDialog();
                    break;
                case ItemType.Battery:
                    inputDialog = new InputDialog("Add Brand", "Enter Battery Brand");
                    inputDialog.ShowDialog();
                    break;
                case ItemType.Tyre:
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
            switch (_itemType)
            {
                case ItemType.Alloywheel:
                    inputDialog = new InputDialog("Add Dimension", "Enter Alloywheel Dimension");
                    inputDialog.ShowDialog();
                    break;
                case ItemType.Battery:
                    inputDialog = new InputDialog("Add Capacity", "Enter Battery Capacity");
                    inputDialog.ShowDialog();
                    break;
                case ItemType.Tyre:
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
            switch (_itemType)
            {
                case ItemType.Battery:
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