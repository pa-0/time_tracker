using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private readonly IProjectTimesData _dataSet = TrackerInstance.DataSet;

        private readonly ObservableCollection<IProject> _projects = new ObservableCollection<IProject>();

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            _dataSet.ProjectsChanged += DataSetOnProjectsChanged;
            this.DataContext = _projects;
        }

        private void DataSetOnProjectsChanged(object sender, object o)
        {
            _projects.Clear();
            foreach (IProject project in _dataSet.Projects)
            {
                _projects.Add(project);
            }
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
             _dataSet.CreateProject("", _newProjectName.Text);
        }

        private void SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGridCellInfo addedCell = e.AddedCells.FirstOrDefault();
            IProject project = (addedCell != default(DataGridCellInfo)) ? (IProject)addedCell.Item : null;

            if (project != null)
            {
                _selectedProject.Content = project.Name;
                _punchInOut.IsEnabled = true;
            }
            else
            {
                _selectedProject.Content = "<None>";
                _punchInOut.IsEnabled = false;
            }
        }

        private void PunchInOutClicked(object sender, RoutedEventArgs e)
        {

        }


    }
}
