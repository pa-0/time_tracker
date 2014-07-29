using System.Windows;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProjectTimesData _dataSet;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            
        }

        private void DeleteClicked(object sender, RoutedEventArgs e)
        {

        }

        private void PunchInOutClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
