using System;
using System.Collections.ObjectModel;
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
        private readonly IProjectTimesData _dataSet = TrackerInstance.DataSet;

        public MainWindow()
        {
            InitializeComponent();

            _dataSet.ProjectsChanged += DataSetOnProjectsChanged;
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            string newName = _newProjectName.Text;
            _dataSet.CreateProject("", newName);
        }

        private void DataSetOnProjectsChanged(object sender, object e)
        {
            _projectsList.Items.Clear();
            foreach (IProject project in _dataSet.Projects)
            {
                _projectsList.Items.Add(project);
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IProject project = (e.AddedItems.Count > 0) ? (IProject)e.AddedItems[0] : null;
            _selectedProject.Content = (project != null) ? project.Name : "<None>";

            UpdateStatus(project);
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
            IProject selectedProject = (IProject)_projectsList.SelectedItem;
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
