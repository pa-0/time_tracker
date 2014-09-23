using System.Windows;
using Ficksworkshop.TimeTracker.Manager;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProjectTimesData _dataSet = TrackerInstance.DataSet;

        private readonly TimeTrackerViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new TimeTrackerViewModel(_dataSet);
            this.DataContext = _vm;
        }
    }
}
