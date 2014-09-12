using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Ficksworkshop.TimeTrackerAPI
{
    public interface ITrackerSetting : INotifyPropertyChanged
    {
        /// <summary>
        /// The internal name of the setting
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get or set the value of the setting
        /// </summary>
        string Value { get; set; }
    }

    public abstract class TrackerSettingBase : ITrackerSetting
    {
        #region Properties

        private readonly string _name;

        public string Name
        {
            get
            {

                return _name;
            }
        }

        #endregion

        #region Constructor

        public TrackerSettingBase(string name)
        {
            _name = name;
        }

        #endregion

        #region ITrackerSetting

        public abstract string Value { get; set; }

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

    public class StringSetting : TrackerSettingBase
    {
        #region Constructor

        public StringSetting(string name)
            : base(name)
        {
        }

        #endregion

        #region Properties

        private string _value;

        public override string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    this.NotifyPropertyChanged("Value");
                }
            }
        }

        #endregion
    }

    public class UintSetting : TrackerSettingBase
    {
        #region Constructor

        public UintSetting(string name) : base(name)
        {
        }

        #endregion

        #region Properties

        private uint _value;

        public override string Value
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
    }

    /// <summary>
    /// This class serves as a bridge to access settings in an enumerable way.
    /// We use this rather than the core type for type conversion, to beautify
    /// the name of properties, and make data binding easy.
    /// </summary>
    public class TrackerSettings
    {
        #region Constants

        public const string ReminderFrequency = "ReminderFrequence";

        public const string MaximumIdleTime = "MaximumIdleTime";

        public const string LastDataSet = "LastDataSet";

        #endregion

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

            var reminderInterval = new UintSetting(ReminderFrequency) { UintValue = 0 };
            var maximumIdleTime = new UintSetting(MaximumIdleTime) { UintValue = 0 };
            var lastDataSet = new StringSetting(LastDataSet) { Value = "" };

            Items.Add(reminderInterval);
            Items.Add(maximumIdleTime);
            Items.Add(lastDataSet);
        }

        #endregion
    }
}
