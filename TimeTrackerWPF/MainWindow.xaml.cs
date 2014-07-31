using System;
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
        private readonly IProjectTimesData _dataSet = TrackerInstance.DataSet;

        private readonly ObservableCollection<IProject> _projects = new ObservableCollection<IProject>();

        private IProject _selectedProjectItem = null;

        public MainWindow()
        {
            InitializeComponent();

            _dataSet.ProjectsChanged += DataSetOnProjectsChanged;
            this.DataContext = _projects;
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            string newName = _newProjectName.Text;
            _dataSet.CreateProject("", newName);
        }

        private void DataSetOnProjectsChanged(object sender, object e)
        {
            _projects.Clear();
            foreach (IProject project in _dataSet.Projects)
            {
                _projects.Add(project);
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedProjectItem = (e.AddedItems.Count > 0) ? (IProject)e.AddedItems[0] : null;
            _selectedProject.Content = (_selectedProjectItem != null) ? _selectedProjectItem.Name : "<None>";

            UpdateStatus(_selectedProjectItem);
        }

        private void UpdateStatus(IProject project)
        {
            IProjectTime time = (project != null) ? _dataSet.FirstOpenTime(project) : null;
            if (project == null || time == null)
            {
                _status.Content = "<None>";
            }
            else
            {
                _status.Content = time.Start;
            }
        }

        private void PunchInOutClicked(object sender, RoutedEventArgs e)
        {
            // Get the selected project
            if (_selectedProjectItem != null)
            {
                IProjectTime time = _dataSet.FirstOpenTime(_selectedProjectItem);
                if (time == null)
                {
                    // Start a new time
                    _dataSet.CreateTime(_selectedProjectItem, DateTime.Now, null);
                }
                else
                {
                    // End the current time
                    time.End = DateTime.Now;
                }

                UpdateStatus(_selectedProjectItem);
            }
        }
    }
}
