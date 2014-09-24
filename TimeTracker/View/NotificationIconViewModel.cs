using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Ficksworkshop.TimeTracker.Manager;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker
{
    public class NotificationIconViewModel : ViewModelBase
    {
        #region Fields

        private SelectedProjectManager _selectedProjectManager;

        #endregion

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

        private ObservableCollection<SelectableProjectViewModel> _activeProjects = new ObservableCollection<SelectableProjectViewModel>();

        /// <summary>
        /// The list of projects that we can punch in or punch out of.
        /// </summary>
        public ObservableCollection<SelectableProjectViewModel> ActiveProjects
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

        private IProject _selectedProject;

        /// <summary>
        /// Gets or sets the selected project to quiickly punch in or out of a project.
        /// </summary>
        public IProject SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                if (value != _selectedProject)
                {
                    // First try to change in the selected project manager, since the change might be rejected
                    // This returns a bool we would want to use to check if project was changed, but
                    // this return false if the project is already that project, which happens if someone else
                    // change the project and this is coming from a notification, so we ignore it, and just check
                    // the property
                    _selectedProjectManager.SetSelectedProject(value);

                    if (_selectedProjectManager.SelectedProject == value)
                    {
                        // Ok, it was changed, so actually update the property
                        _selectedProject = value;
                        NotifyPropertyChanged("SelectedProject");
                    }
                }
            }
        }

        #endregion

        #region Constructors

        public NotificationIconViewModel(SelectedProjectManager selectedProjectManager)
        {
            // When we are constructed, we need to listen to events coming from the data
            // set so that we can update our local view.
            TrackerInstance.DataSetChangedEvent += DataContextChangedEventHandler;
            if (TrackerInstance.DataSet != null)
            {
                TrackerInstance.DataSet.ProjectsChanged += ProjectsChangedEventHandler;
                TrackerInstance.DataSet.ProjectTimeChanged += ProjectTimeChangedEventHandler;
            }

            _selectedProjectManager = selectedProjectManager;
            _selectedProjectManager.SelectedProjectChanged += SelectedProjectChangedEventHandler;
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
                    ActiveProjects.Add(new SelectableProjectViewModel(project, _selectedProjectManager));
                }
            }
        }

        private void ProjectTimeChangedEventHandler(object sender, TimesChangedEventArgs eventArgs)
        {
            // Refresh the is punched in value
            IsPunchedIn = (eventArgs.DataSet.FirstOpenTime() != null);
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

        /// <summary>
        /// Handler when the selected project changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SelectedProjectChangedEventHandler(object sender, SelectedProjectChangedEventArgs eventArgs)
        {
            IProject oldSelection = SelectedProject;

            // This needs to cause the projecs view model to send the notification
            SelectedProject = eventArgs.NewProject;

            // The selectable projects also store a state (for the view) about whether they are selected
            // TODO Maybe the selection should be a selectable project so we don't have to find it
            var oldSelectedViewModel = ActiveProjects.FirstOrDefault(p => p.Project == oldSelection);
            if (oldSelectedViewModel != null)
            {
                oldSelectedViewModel.IsSelected = false;
            }
        }

        #endregion
    }
}