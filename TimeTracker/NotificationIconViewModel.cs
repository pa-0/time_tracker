using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTrackerAPI.Commands;

namespace Ficksworkshop.TimeTracker
{
    public class NotificationIconViewModel : INotifyPropertyChanged
    {
        #region Properties

        private bool _isPunchedIn;

        public bool IsPunchedIn
        {
            get
            {
                return _isPunchedIn;
            }
            private set
            {
                if (value != _isPunchedIn)
                {
                    _isPunchedIn = value;
                    NotifyPropertyChanged("IsPunchedIn");
                }
            }
        }

        private ObservableCollection<IProject> _activeProjects = new ObservableCollection<IProject>();

        /// <summary>
        /// The list of projects that we can punch in or punch out of.
        /// </summary>
        public ObservableCollection<IProject> ActiveProjects
        {
            get
            {
                return _activeProjects;
            }
            set
            {
                if (_activeProjects != value)
                {
                    _activeProjects = value;
                    NotifyPropertyChanged("ActiveProjectsCollection");
                }
            }
        }

        public ICommand TogglePunchStateCommand;

        #endregion

        #region Constructors

        public NotificationIconViewModel()
        {
            // When we are constructed, we need to listen to events coming from the data
            // set so that we can update our local view.
            TrackerInstance.DataSetChangedEvent += DataContextChangedEventHandler;
            if (TrackerInstance.DataSet != null)
            {
                TrackerInstance.DataSet.ProjectsChanged += ProjectsChangedEventHandler;
                TrackerInstance.DataSet.ProjectTimeChanged += ProjectTimeChangedEventHandler;
            }

            TogglePunchStateCommand = new PunchInOutCommand(() => TrackerInstance.DataSet, () => null);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private Members

        private void ProjectsChangedEventHandler(object sender, object e)
        {
            // Refresh the list of active projects
            ActiveProjects.Clear();
            foreach (var project in TrackerInstance.DataSet.Projects)
            {
                if (project.Status != ProjectStatus.Closed)
                {
                    ActiveProjects.Add(project);
                }
            }
        }

        private void ProjectTimeChangedEventHandler(object sender, IProjectTimesData dataSet, IProjectTime modifiedTime)
        {
            // Refresh the is punched in value
            IsPunchedIn = (dataSet.PunchedInTime() != null);
        }

        private void DataContextChangedEventHandler(IProjectTimesData oldDataSet, IProjectTimesData newDataSet)
        {
            // If we completely change the data set, then unsubscribe from the old data set, subscribe to the new
            // one, and update our projects
            if (oldDataSet != null)
            {
                oldDataSet.ProjectsChanged -= ProjectsChangedEventHandler;
                oldDataSet.ProjectTimeChanged -= ProjectTimeChangedEventHandler;
            }
            if (newDataSet != null)
            {
                newDataSet.ProjectsChanged += ProjectsChangedEventHandler;
                newDataSet.ProjectTimeChanged += ProjectTimeChangedEventHandler;
            }

            ProjectsChangedEventHandler(null, null);
        }

        #endregion
    }
}
