using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTrackerAPI.Commands;

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

        public ICommand PunchInOutCommand { get; private set; }

        public TimeTrackerViewModel(IProjectTimesData dataSet)
        {
            _dataSet = dataSet;
            _dataSet.ProjectsChanged += DataSetOnProjectsChanged;
            _dataSet.ProjectTimeChanged += DataSetOnTimesChanged;

            Projects = new ObservableCollection<IProject>();

            PunchInOutCommand = new PunchInOutCommand(() => _dataSet, () => SelectedProject);
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

        private void DataSetOnTimesChanged(object sender, TimesChangedEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            IProjectTime time = (SelectedProject != null) ? _dataSet.FirstOpenTime(SelectedProject) : null;
            string status = (time != null) ? time.Start.ToString() : "<None>";

            Status = status;
        }
    }
}
