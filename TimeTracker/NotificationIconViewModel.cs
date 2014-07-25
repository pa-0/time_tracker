using System.Collections.ObjectModel;
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

        private ObservableCollection<IProject> _activeProjects;

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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
