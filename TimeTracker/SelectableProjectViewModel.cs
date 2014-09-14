using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    public class SelectableProjectViewModel : ViewModelBase
    {
        private SelectedProjectManager _selectionManager;

        private IProject _project;
        public string Name
        {
            get
            {
                return _project.Name;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _selectionManager.SelectedProject == _project;
            }
            set
            {
                if (_selectionManager.SelectedProject != _project && _selectionManager.SetSelectedProject(_project))
                {
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        public SelectableProjectViewModel(IProject project, SelectedProjectManager selectionManager)
        {
            _project = project;
            _selectionManager = selectionManager;
        }
    }
}
