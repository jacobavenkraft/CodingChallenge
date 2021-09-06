using System;
using System.Globalization;
using System.Windows.Data;

namespace CodingChallenge.Framework.Converters
{
    public class ScalingConverter : IValueConverter
    {
        public double ScalingFactor { get; set; } = 2.0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            try
            {
                var doubleValue = System.Convert.ToDouble(value);
                return doubleValue * ScalingFactor;
            }
            catch
            { }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            try
            {
                var doubleValue = System.Convert.ToDouble(value);
                return doubleValue / ScalingFactor;
            }
            catch
            { }

            return value;
        }
    }
}
