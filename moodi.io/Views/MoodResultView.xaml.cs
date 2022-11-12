using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using moodi.io.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace moodi.io.Views
{
    /// <summary>
    /// Interaction logic for MoodResultView.xaml
    /// </summary>
    public partial class MoodResultView : UserControl
    {
        private MainWindow _mainWindow;

        public Func<ChartPoint, string> PointLabel { get; set; }
        public SeriesCollection SeriesCollection { get; set; }


        public int Confidence { get; set; } = 8;
        public int Joy { get; set; } = 4;
        public int Sorrow { get; set; } = 6;
        public int Anger { get; set; } = 1;
        public int Surprise { get; set; } = 20;
        
        public MoodResultView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();



            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Confidence",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Confidence) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Joy",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Joy) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Sorrow",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Sorrow) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Anger",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Anger) },
                    DataLabels = true
                },

                new PieSeries
                {
                    Title = "Surprise",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Surprise) },
                    DataLabels = true,
                }
            };

            PointLabel = chartPoint => $"{chartPoint.Participation:P} %";


            List<Movie> movies = new List<Movie>();

            movies.Add(new Movie(){Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });
            movies.Add(new Movie() { Title = "Title 1", Director = "Director", Duration = "180min", Image = "C:\\Users\\pc\\Desktop\\moodi.io\\moodi.io\\Assets\\Images\\machine-learning.png" });

            
            ListView1.ItemsSource = null;
            ListView1.ItemsSource = movies;

            ListView2.ItemsSource = null;
            ListView2.ItemsSource = movies;

            ListView3.ItemsSource = null;
            ListView3.ItemsSource = movies;


            DataContext = this;
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 10;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ShowFaceDetection();
        }
    }
}
