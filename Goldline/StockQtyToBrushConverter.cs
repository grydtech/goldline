using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace Goldline
{
    internal class StockQtyToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var qty = (uint) value;
            if (qty > 20) return MakeBrush(Color.LightGreen);
            if (qty > 10) return MakeBrush(Color.Gold);
            return qty > 5 ? MakeBrush(Color.LightSalmon) : MakeBrush(Color.IndianRed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static SolidColorBrush MakeBrush(Color color)
        {
            return new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
        }
    }
}