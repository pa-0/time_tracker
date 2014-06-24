using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Ficksworkshop.TimeTracker
{
    public interface IProjectItem
    {
        string Name { get; }

        bool Active { get; }
    }

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

        private IList<IProjectItem> _activeProjects;

        public IList<IProjectItem> ActiveProjects
        {
            get
            {
                if (_activeProjects == null)
                {
                    _activeProjects = new List<IProjectItem>();
                    _activeProjects.Add(new ProjectItem{ Name ="1"});
                    _activeProjects.Add(new ProjectItem { Name = "2" });
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

    public class ProjectItem : IProjectItem
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
