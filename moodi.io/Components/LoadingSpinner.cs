using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace moodi.io.Components
{
    public class LoadingSpinner : Control
    {
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            nameof(IsLoading), typeof(bool), typeof(LoadingSpinner), new PropertyMetadata(true));

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        static LoadingSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingSpinner), new FrameworkPropertyMetadata(typeof(LoadingSpinner)));
        }
    }
}
