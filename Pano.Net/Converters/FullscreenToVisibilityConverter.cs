using System;
using System.Windows;
using System.Windows.Data;

namespace Pano.Net.Converters
{

    /// <summary>
    /// Convert from bool (IsFullscreen) to Visibility
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    class FullscreenToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return true;
        }
    }
}
