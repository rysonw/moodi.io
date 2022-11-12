using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace moodi.io.Converters
{
    public class BoolToInverseVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Visible;

            if ((bool)value == true) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}