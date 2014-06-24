using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    public class NotificationIconViewModel : INotifyPropertyChanged
    {
        private bool _isPunchedIn = false;

        public bool IsPunchedIn
        {
            get
            {
                return _isPunchedIn;
            }
            set
            {
                if (value != _isPunchedIn)
                {
                    _isPunchedIn = value;
                    NotifyPropertyChanged("IsPunchedIn");
                }
            }
        }

        private IList<IProject> _activeProjects;

        public IList<IProject> ActiveProjects
        {
            get
            {
                if (_activeProjects == null)
                {
                    _activeProjects = new List<IProject>();
                    _activeProjects.Add(new Project{ Description ="1"});
                    _activeProjects.Add(new Project { Description = "2" });
                }
                return _activeProjects;
            }
        }

        public ICommand TogglePunchStateCommand
        {
            get
            {
                return new DelegateCommand { CanExecuteFunc = () => true, CommandAction = () => { IsPunchedIn = !IsPunchedIn; } };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
