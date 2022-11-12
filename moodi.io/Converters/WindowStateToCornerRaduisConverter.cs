using System;
using System.Windows;
using System.Windows.Data;

namespace moodi.io.Converters
{

    public class WindowStateToCornerRaduisConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string temp = (string)parameter;

            var values = temp.Split(' ');

            CornerRadius radius = new CornerRadius();
            radius.TopLeft = double.Parse(values[0]);
            radius.TopRight = double.Parse(values[1]);
            radius.BottomRight = double.Parse(values[2]);
            radius.BottomLeft = double.Parse(values[3]);

            bool isMaximized = ((WindowState)value) == WindowState.Maximized;
            if (!isMaximized)
            {
                return radius;
            }
            return new CornerRadius(0);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }

    }
}
