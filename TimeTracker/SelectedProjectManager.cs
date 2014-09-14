using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Event arguments for when the selected project has been changed.
    /// </summary>
    public class SelectedProjectChangedEventArgs
    {
        /// <summary>
        /// The project that was selected.
        /// </summary>
        public IProject OldProject { get; private set; }

        /// <summary>
        /// The project that is currently selected.
        /// </summary>
        public IProject NewProject { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedProjectChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldProject">The project that was previously selected.</param>
        /// <param name="newProject">The project that is now selected.</param>
        public SelectedProjectChangedEventArgs(IProject oldProject, IProject newProject)
        {
            OldProject = oldProject;
            NewProject = newProject;
        }
    }

    /// <summary>
    /// Event arguments for when the selected project is about to be changed. If any event handler sets <see cref="Cancel"/> to true,
    /// then the selected project will not be changed.
    /// </summary>
    public class SelectedProjectChangingEventArgs : SelectedProjectChangedEventArgs
    {
        /// <summary>
        /// If set to true by any event handler, then the selected project will not be changed.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Initializes a new instances of the <see cref="SelectedProjectChangingEventArgs"/> class.
        /// </summary>
        /// <param name="oldProject">The project that was previously selected.</param>
        /// <param name="newProject">The project that is now selected.</param>
        public SelectedProjectChangingEventArgs(IProject oldProject, IProject newProject) : base(oldProject, newProject)
        {
        }
    }

    public delegate void SelectedProjectChanging(object sender, SelectedProjectChangingEventArgs eventArgs);

    public delegate void SelectedProjectChanged(object sender, SelectedProjectChangedEventArgs eventArgs);

    /// <summary>
    /// In the UI, we want to have a quick way to punch in and out of a particular project, without having to pick
    /// from the list of projects which one to punch in and out of. Different parts of the UI should present the same
    /// selected project, so they need a common way to know about which project is selected. This manager stores that
    /// information about which project is selected.
    /// </summary>
    public class SelectedProjectManager
    {
        #region Fields

        private IProject _selectedProject;

        #endregion

        #region Properties

        public IProject SelectedProject { get { return _selectedProject; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedProjectManager"/> class.
        /// </summary>
        public SelectedProjectManager()
        {
            // TODO I don't really like this, but I don't have a better idea for now
            TrackerInstance.DataSetChangedEvent += DataSetChangedEvent;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Tries to change to the selected project. The selected project may not be changed if an event handler
        /// cancels the action.
        /// </summary>
        /// <param name="project">The project to select.</param>
        /// <returns>True if the project was changed, otherwise false.</returns>
        public bool SetSelectedProject(IProject project)
        {
            return SetSelectedProjectInternal(project, true);
        }

        #endregion

        #region Private Members

        private void DataSetChangedEvent(IProjectTimesData oldDataSet, IProjectTimesData newDataSet)
        {
            SetSelectedProjectInternal(null, false);
        }

        /// <summary>
        /// Tries to change to the selected project. The selected project may not be changed if an event handler
        /// cancels the action.
        /// </summary>
        /// <param name="project">The project to select.</param>
        /// <param name="sendChanging">If true, the action can be canceled, otherwise false.</param>
        /// <returns>True if the project was changed, otherwise false.</returns>
        public bool SetSelectedProjectInternal(IProject project, bool sendChanging)
        {
            if (project != _selectedProject)
            {
                // First, tell everyone things are about to change
                var changingArgs = new SelectedProjectChangingEventArgs(_selectedProject, project);

                if (sendChanging && SelectedProjectChanging != null)
                {
                    SelectedProjectChanging.Invoke(null, changingArgs);
                }

                if (!changingArgs.Cancel)
                {
                    // Create the args first, before changing the project
                    var changedArgs = new SelectedProjectChangedEventArgs(_selectedProject, project);

                    // Change the selection
                    _selectedProject = project;

                    // And tell everyone it happened
                    if (SelectedProjectChanged != null)
                    {
                        SelectedProjectChanged.Invoke(null, changedArgs);
                    }

                    return true;
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Event fires whenever the selected project is about to be changed and the change can be canceled.
        /// The event doesn't fire if the change can't be canceled.
        /// </summary>
        public event SelectedProjectChanging SelectedProjectChanging;

        /// <summary>
        /// Event fires whenever the selected project has been changed.
        /// </summary>
        public event SelectedProjectChanged SelectedProjectChanged;
    }
}
