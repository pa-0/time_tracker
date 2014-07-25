using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Ficksworkshop.TimeTrackerAPI
{
    public interface ITrackerSetting : INotifyPropertyChanged
    {
        string Name { get; }

        string Value { get; set; }
    }

    public class UintSetting : ITrackerSetting
    {
        #region Constructor

        public UintSetting(string name)
        {
            _name = name;
        }

        #endregion

        #region Properties

        private readonly string _name;

        public string Name
        {
            get
            {
                
                return _name;
            }
        }

        private uint _value;

        public string Value
        {
            get
            {
                return UintValue.ToString();
            }
            set
            {
                uint uintValue;
                if (uint.TryParse(value, out uintValue))
                {
                    UintValue = uintValue;
                }
            }
        }

        public uint UintValue
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.NotifyPropertyChanged("Value");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    /// <summary>
    /// This class serves as a bridge to access settings in an enumerable way.
    /// We use this rather than the core type for type conversion, to beautify
    /// the name of properties, and make data binding easy.
    /// </summary>
    public class TrackerSettings
    {
        #region Properties

        public ObservableCollection<ITrackerSetting> Items { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerSettings"/> class.
        /// </summary>
        public TrackerSettings()
        {
            Items = new ObservableCollection<ITrackerSetting>();

            // Populate the settings from the actual properties

            var reminderInterval = new UintSetting("ReminderFrequency") { UintValue = 0 };
            var maximumIdleTime = new UintSetting("MaximumIdleTime") { UintValue = 0 };

            Items.Add(reminderInterval);
            Items.Add(maximumIdleTime);
        }

        #endregion
    }
}
