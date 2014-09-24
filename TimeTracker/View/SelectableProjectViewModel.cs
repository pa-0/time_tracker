using System;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker.View
{
    /// <summary>
    /// A view of projects for the context menu. Contains a way to ask if the project is selected, but
    /// the implementation is fundamentally broken.
    /// </summary>
    public class SelectableProjectViewModel : ViewModelBase
    {
        private SelectedProjectManager _selectionManager;

        private IProject _project;

        public IProject Project { get { return _project;  } }

        /// <summary>
        /// Gets a display name for the project.
        /// </summary>
        public string Name
        {
            get
            {
                return _project.Name;
            }
        }
        
        /// <summary>
        /// Sets if this project is currently selected. There is no way to "unset" as selected, so
        /// unsetting just sends a notification.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _selectionManager.SelectedProject == _project;
            }
            set
            {
                // TODO I still don't like this implementation
                if (value)
                {
                    if (_selectionManager.SelectedProject != _project && _selectionManager.SetSelectedProject(_project))
                    {
                        NotifyPropertyChanged("IsSelected");
                    }
                }
                else
                {
                    NotifyPropertyChanged("IsSelected");
                }
                
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableProjectViewModel"/> class.
        /// </summary>
        /// <param name="project">The project this is representing.</param>
        /// <param name="selectionManager">The project selection manager.</param>
        public SelectableProjectViewModel(IProject project, SelectedProjectManager selectionManager)
        {
            _project = project;
            _selectionManager = selectionManager;
        }
    }
}
