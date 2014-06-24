using System.Windows;

using Ficksworkshop.TimeTracker.Properties;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SettingsGrid.DataContext = TrackerSettings.Default;
            SettingsGrid.ItemsSource = TrackerSettings.Default.Items;
        }
    }
}
