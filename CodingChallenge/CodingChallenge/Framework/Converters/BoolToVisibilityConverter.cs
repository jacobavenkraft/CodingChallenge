using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CodingChallenge.Framework.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public BoolToVisibilityConverter()
        {

        }

        public Visibility WhenTrue { get; set; } = Visibility.Visible;

        public Visibility WhenFalse { get; set; } = Visibility.Hidden;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? WhenTrue : WhenFalse;
            }

            return value;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }        
    }
}
