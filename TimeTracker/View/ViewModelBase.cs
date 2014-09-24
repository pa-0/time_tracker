using System.ComponentModel;
namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Base implementation of <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
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
