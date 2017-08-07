using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Goldline
{
    public static class Extensions
    {
        public static void MakeComboBoxSearchable(this ComboBox targetComboBox)
        {
            targetComboBox.Loaded += TargetComboBox_Loaded;
        }

        private static void TargetComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var targetComboBox = sender as ComboBox;
            var targetTextBox = targetComboBox?.Template.FindName("PART_EditableTextBox", targetComboBox) as TextBox;

            if (targetTextBox == null) return;
            if (targetTextBox.Tag?.ToString() == "FirstTimeInitialized") return;

            targetComboBox.Tag = "TextInput";

            // When handlers registered for the first time, this tag is set. It makes sure the handlers do not reregister
            targetTextBox.Tag = "FirstTimeInitialized";

            targetTextBox.TextChanged +=
                (o, args) => TargetTextBoxOnTextChanged(o, targetComboBox);

            targetComboBox.SelectionChanged += OnTargetComboBoxOnSelectionChanged;
        }

        private static void OnTargetComboBoxOnSelectionChanged(object o, SelectionChangedEventArgs args)
        {
            var comboBox = o as ComboBox;
            if (comboBox?.SelectedItem == null) return;
            comboBox.Tag = "Selection";
        }

        private static void TargetTextBoxOnTextChanged(object o, ComboBox targetComboBox)
        {
            var textBox = (TextBox) o;

            var searchText = textBox.Text;

            if (targetComboBox.Tag.ToString() == "Selection")
            {
                targetComboBox.Tag = "TextInput";
            }
            else
            {
                if (targetComboBox.SelectionBoxItem != null)
                {
                    targetComboBox.SelectedItem = default(object);
                    textBox.Text = searchText;
                    textBox.CaretIndex = int.MaxValue;
                }

                if (string.IsNullOrEmpty(searchText))
                {
                    targetComboBox.Items.Filter = item => true;
                    targetComboBox.SelectedItem = default(object);
                }
                else
                {
                    targetComboBox.Items.Filter = item =>
                        item.ToString().StartsWith(searchText, true, CultureInfo.InvariantCulture);
                }

                // If exact match found for string, select that item
                targetComboBox.SelectedItem =
                    targetComboBox.ItemsSource?.Cast<object>().FirstOrDefault(i => i.ToString() == searchText);
                textBox.CaretIndex = int.MaxValue;
                targetComboBox.Tag = "TextInput";
            }
        }
    }
}