using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Domain.Handlers;
using Core.Domain.Model.Suppliers;
using Goldline.UI.Suppliers.Dialogs;
using Control = System.Windows.Forms.Control;

namespace Goldline.UI.Suppliers
{
    /// <summary>
    ///     Interaction logic for PurchaseHistoryWindow.xaml
    /// </summary>
    public partial class PurchaseHistoryWindow : Window
    {
        private readonly PurchaseHandler _purchaseHandler;

        public PurchaseHistoryWindow()
        {
            _purchaseHandler = new PurchaseHandler();
            PurchaseSource = _purchaseHandler.GetPurchases();
            InitializeComponent();
        }

        public IEnumerable<Purchase> PurchaseSource { get; set; }

        public void RefreshDataGrid()
        {
            PurchaseSource = _purchaseHandler.GetPurchases(note: SearchTextBox.Text);
            PurchasesDataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
        }

        public Point GetMousePosition()
        {
            var point = Control.MousePosition;
            return new Point(point.X, point.Y);
        }

        private void ViewSupplyOrders_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (PurchasesDataGrid.SelectedIndex < PurchasesDataGrid.Items.Count - 1)
                        PurchasesDataGrid.SelectedIndex++;
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (PurchasesDataGrid.SelectedIndex > 0) PurchasesDataGrid.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        #region Button Click

        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            if (PurchasesDataGrid.SelectedItem == null) return;
            var result = MessageBox.Show("Confirm Supply Order", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            try
            {
                var purchase = (Purchase) PurchasesDataGrid.SelectedItem;
                if (purchase.Id == null) return;
                _purchaseHandler.DeletePurchase(purchase.Id.Value);
                MessageBox.Show("Successfully Reversed", "Successful", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                RefreshDataGrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Not Reversed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            new AddPurchaseDialog().ShowDialog();
            RefreshDataGrid();
        }

        #endregion

        private void PurchasesDataGrid_OnRowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
        {
            if (PurchasesDataGrid.SelectedItem == null) return;
            if (((Purchase) PurchasesDataGrid.SelectedItem).PurchaseItems.Count > 0) return;
            _purchaseHandler.LoadPurchaseItems((Purchase) PurchasesDataGrid.SelectedItem);
            PurchasesDataGrid.Items.Refresh();
        }
    }
}