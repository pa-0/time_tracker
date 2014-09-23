using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTracker.Model;

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

        private IProject _selectedProjectItem = null;

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
            IProject selectedProject = _selectedProjectItem;
            if (selectedProject != null)
            {
                IProjectTime time = _dataSet.FirstOpenTime(selectedProject);
                if (time == null)
                {
                    // Start a new time
                    _dataSet.CreateTime(selectedProject, DateTime.Now, null);
                }
                else
                {
                    // End the current time
                    time.End = DateTime.Now;
                }

                UpdateStatus(selectedProject);
            }
        }
    }
}
