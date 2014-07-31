using System.ComponentModel;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Base class implementation of the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <inhertdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inhertdoc />
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
