using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace moodi.io.Converters
{

    public class StringToResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                if (value == null) return null;

                if (value is string && value.ToString() != "")
                {
                    return Application.Current.FindResource(value.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
