using System.Windows;
using Goldline.UI.Catalogs;

namespace Goldline.UI.Products
{
    /// <summary>
    ///     Interaction logic for CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        public CatalogWindow()
        {
            InitializeComponent();
        }

        private void BatteryButton_Click(object sender, RoutedEventArgs e)
        {
            new BatteryCatalog().Show();
        }

        private void TyreButton_Click(object sender, RoutedEventArgs e)
        {
            new TyreCatalog().Show();
        }

        private void AlloyWheelButton_Click(object sender, RoutedEventArgs e)
        {
            new AlloywheelCatalog().Show();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}