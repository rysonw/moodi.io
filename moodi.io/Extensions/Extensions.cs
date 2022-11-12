using System.Windows;
using System.Windows.Media;

namespace moodi.io.Extensions
{
    public class Extensions
    {

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(string), typeof(Extensions), new PropertyMetadata(""));


        public static void SetIcon(UIElement element, string value)
        {
            element.SetValue(IconProperty, value);
        }

        public static string GetIcon(UIElement element)
        {
            return (string)element.GetValue(IconProperty);
        }


        public static readonly DependencyProperty HintProperty =
            DependencyProperty.RegisterAttached("Hint", typeof(string), typeof(Extensions), new PropertyMetadata(""));


        public static void SetHint(UIElement element, string value)
        {
            element.SetValue(HintProperty, value);
        }

        public static string GetHint(UIElement element)
        {
            return (string)element.GetValue(HintProperty);
        }


        public static readonly DependencyProperty PrefixProperty =
            DependencyProperty.RegisterAttached("Prefix", typeof(string), typeof(Extensions), new PropertyMetadata(""));


        public static void SetPrefix(UIElement element, string value)
        {
            element.SetValue(PrefixProperty, value);
        }

        public static string GetPrefix(UIElement element)
        {
            return (string)element.GetValue(PrefixProperty);
        }


        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(Extensions), new PropertyMetadata(new CornerRadius(0, 0, 0, 0)));


        public static void SetCornerRadius(UIElement element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(UIElement element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }


        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.RegisterAttached("BackgroundColor", typeof(Brush), typeof(Extensions), new PropertyMetadata(null));


        public static void SetBackgroundColor(UIElement element, Brush value)
        {
            element.SetValue(BackgroundColorProperty, value);
        }

        public static Brush GetBackgroundColor(UIElement element)
        {
            return (Brush)element.GetValue(BackgroundColorProperty);
        }

    }
}
