using System;
using System.Collections.ObjectModel;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    public class TimeTrackerViewModel : ViewModelBase
    {
        private readonly IProjectTimesData _dataSet;

        public ObservableCollection<IProject> Projects { get; private set; }

        public string NewProjectName { get; set; }

        private IProject _selectedProject;

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

        private string _status;

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

        public void AddClicked()
        {
            string newName = NewProjectName;
            _dataSet.CreateProject("", newName);
        }

        private void DataSetOnProjectsChanged(object sender, object e)
        {
            Projects.Clear();
            foreach (IProject project in _dataSet.Projects)
            {
                Projects.Add(project);
            }
        }

        private void UpdateStatus()
        {
            IProjectTime time = (SelectedProject != null) ? _dataSet.FirstOpenTime(SelectedProject) : null;
            string status = (time != null) ? time.Start.ToString() : "<None>";

            Status = status;
        }

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
