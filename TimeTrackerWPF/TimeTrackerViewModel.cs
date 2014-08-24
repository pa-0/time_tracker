using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    public class TimeTrackerViewModel : ViewModelBase
    {
        private readonly IProjectTimesData _dataSet;

        public ObservableCollection<IProject> Projects { get; private set; }

        private IProject _selectedProject;

        public IProject SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                if (value != _selectedProject)
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
            set
            {
                if (value != _status)
                {
                    _status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        private string _newProjectName;
        public string NewProjectName
        {
            get
            {
                return _newProjectName;
            }
            set
            {
                if (value != _newProjectName)
                {
                    _newProjectName = value;
                    NotifyPropertyChanged("NewProjectName");
                }
            }
        }

        public TimeTrackerViewModel(IProjectTimesData dataSet)
        {
            _dataSet = dataSet;

            Projects = new ObservableCollection<IProject>();

            _dataSet.ProjectsChanged += DataSetOnProjectsChanged;
        }

        public void AddClicked()
        {
            _dataSet.CreateProject("", NewProjectName);
        }

        public void PunchInOutClicked()
        {
            // Get the selected project
            IProject selectedProject = SelectedProject;
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

                UpdateStatus();
            }
        }

        private void DataSetOnProjectsChanged(object sender, object o)
        {
            Projects.Clear();
            foreach (IProject project in _dataSet.Projects)
            {
                Projects.Add(project);
            }
        }

        private void UpdateStatus()
        {
            IProjectTime time = (_selectedProject != null) ? _dataSet.FirstOpenTime(_selectedProject) : null;
            if (_selectedProject == null || time == null)
            {
                Status = "<None>";
            }
            else
            {
                Status = time.Start.ToString();
            }
        }
    }
}
