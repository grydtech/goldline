using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Goldline
{
    /// <summary>
    ///     Interaction logic for PersonComboBox.xaml
    /// </summary>
    public partial class PersonComboBox : UserControl, INotifyPropertyChanged
    {
        // Using a DependencyProperty as the backing store for ColumnHeader1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnHeader1Property =
            DependencyProperty.Register("ColumnHeader1", typeof(string), typeof(PersonComboBox),
                new UIPropertyMetadata(string.Empty, TwoColumnComboBoxControlPropertyChangedCallback));

        // Using a DependencyProperty as the backing store for ColumnHeader2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnHeader2Property =
            DependencyProperty.Register("ColumnHeader2", typeof(string), typeof(PersonComboBox),
                new UIPropertyMetadata(string.Empty, TwoColumnComboBoxControlPropertyChangedCallback));

        // Using a DependencyProperty as the backing store for Watermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(PersonComboBox),
                new UIPropertyMetadata(string.Empty, TwoColumnComboBoxControlPropertyChangedCallback));

        // Using a DependencyProperty as the backing store for WatermarkBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkBrushProperty =
            DependencyProperty.Register("WatermarkBrush", typeof(Brush), typeof(PersonComboBox),
                new PropertyMetadata(Brushes.Gray));

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PersonComboBox),
                new PropertyMetadata(null, TwoColumnComboBoxControlPropertyChangedCallback));


        private object _selectedItem;


        public PersonComboBox()
        {
            InitializeComponent();
            SearchComboBox.MakeComboBoxSearchable();
        }


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string ColumnHeader1
        {
            get { return (string) GetValue(ColumnHeader1Property); }
            set { SetValue(ColumnHeader1Property, value); }
        }


        public string ColumnHeader2
        {
            get { return (string) GetValue(ColumnHeader2Property); }
            set { SetValue(ColumnHeader2Property, value); }
        }

        public Brush WatermarkBrush
        {
            get { return (Brush) GetValue(WatermarkBrushProperty); }
            set { SetValue(WatermarkBrushProperty, value); }
        }

        public string Watermark
        {
            get { return (string) GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public string Text
        {
            get { return SearchComboBox.Text; }
            set
            {
                SearchComboBox.Text = value;
                OnPropertyChanged();
            }
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void TwoColumnComboBoxControlPropertyChangedCallback(DependencyObject o,
            DependencyPropertyChangedEventArgs args)
        {
            ((PersonComboBox) o).OnPropertyChanged(nameof(args.Property));
        }

        public event SelectionChangedEventHandler SelectionChanged;
        public event EventHandler TextChanged;
        public event RoutedEventHandler SelectionConfirmed;

        private void SearchComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = SearchComboBox.SelectedItem;
            SelectionChanged?.Invoke(sender, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchComboBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private void PART_EditableTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(@"got focus");
            Dispatcher.BeginInvoke(new Action(() => SearchComboBox.IsDropDownOpen = true));
        }

        private void PART_EditableTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(@"lost focus");
            Dispatcher.BeginInvoke(new Action(() => SearchComboBox.IsDropDownOpen = false));
            if (SelectedItem != null) SelectionConfirmed?.Invoke(this, e);
        }

        private void SearchComboBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SelectedItem != null)
                SelectionConfirmed?.Invoke(this, e);
        }
    }
}