using CodingChallenge.Transport;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CodingChallenge.Framework.Converters
{
    public class TransportStatusToVisibilityConverter : IValueConverter
    {
        public TransportStatus TargetStatus { get; set; }

        public Visibility WhenEqualTargetStatus { get; set; } = Visibility.Visible;

        public Visibility WhenNotEqualTargetStatus { get; set; } = Visibility.Hidden;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TransportStatus transportStatusValue)
            {
                return (transportStatusValue == TargetStatus) ? WhenEqualTargetStatus : WhenNotEqualTargetStatus;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
