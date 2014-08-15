using System.Collections.ObjectModel;
using System.Windows.Input;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTrackerAPI.Commands;

namespace Ficksworkshop.TimeTracker
{
    public class TimeTrackerViewModel : ViewModelBase
    {
        private readonly IProjectTimesData _dataSet;

        /// <summary>
        /// All known projects that we are going to show.
        /// </summary>
        public ObservableCollection<IProject> Projects { get; private set; }
        
        /// <summary>
        /// If creating a new project, this is the name of the new project. Don't need
        /// to implement NotifyPropertyChanged here because it is only set by the view.
        /// </summary>
        public string NewProjectName { get; set; }

        
        private IProject _selectedProject;

        /// <summary>
        /// The single currently selected project from <see cref="Projects"/>.
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

        private string _status;

        /// <summary>
        /// The status information for the currently selected project. We could take
        /// this futher by using a value converter, but that's outside the scope of this
        /// demonstration.
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

        /// <summary>
        /// Bindable command to create a new project.
        /// </summary>
        public ICommand CreateProjectCommand { get; private set; }

        /// <summary>
        /// Bindable command to punch in/out of the selected project.
        /// </summary>
        public ICommand PunchInOutCommand { get; private set; }

        /// <summary>
        /// Initalizes a new instance of the <see cref="TimeTrackerViewModel"/> class.
        /// </summary>
        /// <param name="dataSet">The data to adapt.</param>
        public TimeTrackerViewModel(IProjectTimesData dataSet)
        {
            _dataSet = dataSet;
            _dataSet.ProjectsChanged += DataSetOnProjectsChanged;
            _dataSet.ProjectTimeChanged += DataSetOnTimesChanged;

            Projects = new ObservableCollection<IProject>();

            CreateProjectCommand = new CreateProjectCommand(() => _dataSet, null, () => NewProjectName);
            PunchInOutCommand = new PunchInOutCommand();
        }

        /// <summary>
        /// Listens to the ProjectsChanged event from the model so we can notify the view.
        /// </summary>
        /// <param name="sender">Default parameter</param>
        /// <param name="e">Information about the project that was changed.</param>
        private void DataSetOnProjectsChanged(object sender, object e)
        {
            Projects.Clear();
            foreach (IProject project in _dataSet.Projects)
            {
                Projects.Add(project);
            }
        }

        /// <summary>
        /// Listens to the ProjectTimeChanged event from the model so we can notify the view.
        /// </summary>
        /// <param name="sender">Default parameter</param>
        /// <param name="e">Information about the time that was changed.</param>
        private void DataSetOnTimesChanged(object sender, TimesChangedEventArgs e)
        {
            UpdateStatus();
        }

        /// <summary>
        /// Helper function to calculate status information. This really should be using a value converter.
        /// </summary>
        private void UpdateStatus()
        {
            IProjectTime time = (SelectedProject != null) ? _dataSet.FirstOpenTime(SelectedProject) : null;
            string status = (time != null) ? time.Start.ToString() : "<None>";

            Status = status;
        }
    }
}
