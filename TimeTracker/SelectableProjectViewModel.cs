using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker
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

        public SelectableProjectViewModel(IProject project, SelectedProjectManager selectionManager)
        {
            _project = project;
            _selectionManager = selectionManager;
        }
    }
}
