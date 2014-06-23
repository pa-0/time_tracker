using System.ComponentModel;
using System.Windows.Input;

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
