using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTracker.Commands;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker
{
    public class TimeTrackerViewModel : ViewModelBase
    {
        private readonly IProjectTimesData _dataSet;

        /// <summary>
        /// Gets the list of projects to display.
        /// </summary>
        public ObservableCollection<IProject> Projects { get; private set; }

        private IProject _selectedProject;

        /// <summary>
        /// Gets or sets the currently selected project in the project list.
        /// </summary>
        public IProject SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    NotifyPropertyChanged("SelectedProject");

                    UpdateStatus();
                }
            }
        }

        private string _newProjectName;

        /// <summary>
        /// Bindable property for the new project name.
        /// 
        /// Since we don't set this in the dialog, I've omitted the notification.
        /// </summary>
        public string NewProjectName
        {
            get
            {
                return _newProjectName;
            }
            set
            {
                if (_newProjectName != value)
                {
                    _newProjectName = value;
                    NotifyPropertyChanged("NewProjectName");
                }
            }
        }

        private string _status;

        /// <summary>
        /// Gets a string that describes the status of the selected project.
        /// </summary>
        public string Status
        {
            get
            {
                return _status;
            }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        public TimeTrackerViewModel(IProjectTimesData dataSet)
        {
            _dataSet = dataSet;
            _dataSet.ProjectsChanged += DataSetOnProjectsChanged;

            Projects = new ObservableCollection<IProject>();
        }

        /// <summary>
        /// Adds a new project to the data set.
        /// </summary>
        public void AddClicked()
        {
            string newName = NewProjectName;
            _dataSet.CreateProject("", newName);
        }

        /// <summary>
        /// Event handler for when the the list of projects in the data set changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataSetOnProjectsChanged(object sender, object e)
        {
            Projects.Clear();
            foreach (IProject project in _dataSet.Projects)
            {
                Projects.Add(project);
            }
        }

        /// <summary>
        /// Helper function to determine the correct status value. It would be possible to use a value converter for this value.
        /// </summary>
        private void UpdateStatus()
        {
            IProjectTime time = (SelectedProject != null) ? _dataSet.FirstOpenTime(SelectedProject) : null;
            string status = (time != null) ? time.Start.ToString() : "<None>";

            Status = status;
        }

        /// <summary>
        /// Punches in or out of the selected project.
        /// </summary>
        public void PunchInOutClicked()
        {
            // Get the selected project
            if (SelectedProject != null)
            {
                IProjectTime time = _dataSet.FirstOpenTime(SelectedProject);
                if (time == null)
                {
                    // Start a new time
                    _dataSet.CreateTime(SelectedProject, DateTime.Now, null);
                }
                else
                {
                    // End the current time
                    time.End = DateTime.Now;
                }

                UpdateStatus();
            }
        }
    }
}
