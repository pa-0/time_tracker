using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    public delegate void ProjectsChangedEventHandler(IProjectTimesData oldDataSet, IProjectTimesData newDataSet);

    /// <summary>
    /// This application allows you to have a single set of time information. This
    /// is that single instance and how you access it. This essentially maanages the
    /// singleton outside of the API project, bucause the API allows multiple instances
    /// of tracker data.
    /// </summary>
    public class TrackerInstance
    {
        #region Properties

        private static IProjectTimesData dataSet;

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
            private set
            {
                dataSet = value;
            }
        }

        private static TrackerSettings settings;

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
            private set
            {
                settings = value;
            }
        }

        public static event ProjectsChangedEventHandler DataSetChangedEvent;

        #endregion
    }
}
