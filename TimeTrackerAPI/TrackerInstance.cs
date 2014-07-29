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

        private static IProjectTimesData dataSet;

        /// <summary>
        /// Gets the singleton data set instance that we are currently using in the application.
        /// </summary>
        public static IProjectTimesData DataSet
        {
            get
            {
                if (dataSet == null)
                {
                    dataSet = new XmlDataSetProjectTimesData(null);

                    if (DataSetChangedEvent != null)
                    {
                        DataSetChangedEvent.Invoke(null, dataSet);
                    }
                }
                return dataSet;
            }
        }

        private static TrackerSettings settings;

        /// <summary>
        /// Gets the singleton settings instance that we are currently using in the application.
        /// </summary>
        public static TrackerSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new TrackerSettings();
                }
                return settings;
            }
        }

        /// <summary>
        /// Event fires whenever the data set instance changes.
        /// </summary>
        public static event DataSetEventHandler DataSetChangedEvent;

        #endregion
    }
}
