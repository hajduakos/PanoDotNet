using System;
using System.Windows;
using System.Windows.Data;

namespace Pano.Net.Converters
{
    /// <summary>
    /// Convert from bool (IsFullscreen) to WindowStyle
    /// </summary>
    [ValueConversion(typeof(bool), typeof(WindowStyle))]
    class FullscreenToWindowStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue) return WindowStyle.None;
            else return WindowStyle.SingleBorderWindow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return true;
        }
    }
}
