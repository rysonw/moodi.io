using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace moodi.io.Views
{
    /// <summary>
    /// Interaction logic for FaceDetectionView.xaml
    /// </summary>
    public partial class FaceDetectionView : UserControl
    {

        private readonly MainWindow _mainWindow;
        public FaceDetectionView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        
        private async void SaveAndEvaluateBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Visibility = Visibility.Collapsed;
            Spinner.Visibility = Visibility.Visible;

            for (int i = 0; i < 300; i++)
            {
                await Task.Delay(10);
            }

            ContentArea.Visibility = Visibility.Visible;
            Spinner.Visibility = Visibility.Collapsed;

            
            _mainWindow.ShowMoodResults();
        }
    }
}
