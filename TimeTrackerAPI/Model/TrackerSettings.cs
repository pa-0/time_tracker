using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Ficksworkshop.TimeTracker.Model
{
    /// <summary>
    /// A simple setting.
    /// </summary>
    public interface ITrackerSetting : INotifyPropertyChanged
    {
        /// <summary>
        /// The viewable name of the setting. This value may be translated
        /// and shouldn't be used as a lookup key.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get or set the value of the setting
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// The key for serialization of this setting.
        /// </summary>
        string SerializationKey { get; }
    }

    /// <summary>
    /// Useful common base class for settings.
    /// </summary>
    public abstract class TrackerSettingBase : ITrackerSetting
    {
        #region Properties

        private readonly string _name;

        /// <inheritdoc />
        public string Name
        {
            get
            {

                return _name;
            }
        }

        /// <inheritdoc />
        public string SerializationKey
        {
            get
            {

                // TODO for now, no translation, no beautification
                return _name;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerSettingBase"/> class.
        /// </summary>
        /// <param name="name"></param>
        public TrackerSettingBase(string name)
        {
            _name = name;
        }

        #endregion

        #region ITrackerSetting

        /// <inheritdoc />
        public abstract string Value { get; set; }

        #endregion

        #region INotifyPropertyChanged

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
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
    /// A setting backed by a string value.
    /// </summary>
    public class StringSetting : TrackerSettingBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSetting"/> class.
        /// </summary>
        /// <param name="name">The key and display name, all in one.</param>
        public StringSetting(string name)
            : base(name)
        {
        }

        #endregion

        #region Properties

        private string _value;

        /// <inheritdoc />
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

    /// <summary>
    /// A setting backed by a unsigned integer value.
    /// </summary>
    public class UintSetting : TrackerSettingBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UintSetting"/> class.
        /// </summary>
        /// <param name="name">The key and display name, all in one.</param>
        public UintSetting(string name) : base(name)
        {
        }

        #endregion

        #region Properties

        private uint _value;

        /// <inheritdoc />
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

        /// <summary>
        /// Gets or sets the value as a uint.
        /// </summary>
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

        /// <summary>
        /// Gets the list of settings for the application.
        /// </summary>
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
