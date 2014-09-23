using System.IO;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTrackerAPI
{
    public delegate void DataSetEventHandler(IProjectTimesData oldDataSet, IProjectTimesData newDataSet);

    /// <summary>
    /// This application allows you to have a single set of time information. This
    /// is that single instance and how you access it. This essentially maanages the
    /// singleton outside of the API project, bucause the API allows multiple instances
    /// of tracker data.
    /// </summary>
    /// <remarks>This code doesn't really belong in the the API project. It exists here
    /// so that the demonstration projects can make use of it (without copying the code
    /// or creating another assembly, which would complicate the demonstration).</remarks>
    public class TrackerInstance
    {
        #region Properties

        private static IProjectTimesData _dataSet;

        /// <summary>
        /// Gets the singleton data set instance that we are currently using in the application.
        /// </summary>
        public static IProjectTimesData DataSet
        {
            get
            {
                if (_dataSet == null)
                {
                    _dataSet = new XmlDataSetProjectTimesData(null);

                    if (DataSetChangedEvent != null)
                    {
                        DataSetChangedEvent.Invoke(null, _dataSet);
                    }
                }
                return _dataSet;
            }
            private set
            {
                if (_dataSet != value)
                {
                    _dataSet = value;

                    if (DataSetChangedEvent != null)
                    {
                        DataSetChangedEvent.Invoke(null, _dataSet);
                    }
                }
            }
        }

        private static TrackerSettings _settings;

        /// <summary>
        /// Gets the singleton settings instance that we are currently using in the application.
        /// </summary>
        public static TrackerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new TrackerSettings();
                }
                return _settings;
            }
        }

        /// <summary>
        /// Event fires whenever the data set instance changes.
        /// </summary>
        public static event DataSetEventHandler DataSetChangedEvent;

        #endregion

        #region Public Members

        public static IProjectTimesData OpenDataSet(string fileName)
        {
            Stream openStream = null;
            if (File.Exists(fileName))
            {
                openStream = new FileStream(fileName, FileMode.OpenOrCreate);
            }
            
            var newDataSet = new XmlDataSetProjectTimesData(openStream);

            DataSet = newDataSet;

            return newDataSet;
        }

        #endregion
    }
}
