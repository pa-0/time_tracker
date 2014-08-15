﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTrackerAPI.Commands;

namespace Ficksworkshop.TimeTracker
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// The data set. This exists because the add/delete commands need this
        /// in order to set the command parameter.
        /// </summary>
        public IProjectTimesData DataSet { get; private set; }

        /// <summary>
        /// The complete list of all projects.
        /// </summary>
        public ObservableCollection<IProject> Projects { get; private set; }

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
                }
            }
        }

        /// <summary>
        /// The complete list of all user-controlled setings
        /// </summary>
        public ObservableCollection<ITrackerSetting> Settings { get; private set; }

        

        public ICommand DeleteProjectCommand { get; private set; }

        public ICommand PunchInOutCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="settings">The settings instance that will be displayed.</param>
        /// <param name="dataSet">The project/times data to display.</param>
        public MainWindowViewModel(TrackerSettings settings, IProjectTimesData dataSet)
        {
            DataSet = dataSet;
            // Create the initial collection that the UI will bind to
            Projects = new ObservableCollection<IProject>();
            RefreshProjects(null, null);

            // Subscribe to the events from the projects so we know if we need to refresh the projects list
            // TODO this leaks because we never unsubscribe
            dataSet.ProjectsChanged += RefreshProjects;

            // Settings
            Settings = settings.Items;

            // Commands
            DeleteProjectCommand = new DeleteProjectCommand();
            PunchInOutCommand = new PunchInOutCommand();
        }

        #endregion

        #region Private Members

        private void RefreshProjects(object sender, object e)
        {
            Projects.Clear();
            foreach (IProject project in DataSet.Projects)
            {
                Projects.Add(project);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
