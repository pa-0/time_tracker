using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Ficksworkshop.TimeTrackerAPI
{
    /// <summary>
    /// Implementation of the <see cref="IProjectTimesData"/> that is backed by an XmlDataSet.
    /// 
    /// This is the representation of the project data so that the main application has no way to know
    /// that we are string information in XML.
    /// </summary>
    public class XmlDataSetProjectTimesData : IProjectTimesData
    {
        #region Properties

        /// <summary>
        /// The actual backing XML data set.
        /// </summary>
        private TimesDataSet DataSet { get; set; }

        private XmlDataSetProjectProxyFactory _projectProxyFactory;

        private XmlDataSetProjectTimeProxyFactory _timeProxyFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSetProjectTimesData"/> class.
        /// </summary>
        /// <param name="inputStream">Input stream to read XML data from. May be null.</param>
        public XmlDataSetProjectTimesData(Stream inputStream)
        {
            LoadDatabase(inputStream);

            _projectProxyFactory = new XmlDataSetProjectProxyFactory(this, DataSet.Projects);

            _timeProxyFactory = new XmlDataSetProjectTimeProxyFactory(this, DataSet.Times);
        }

        #endregion

        #region IProjectTimesData Members

        /// <inheritdoc/>
        public IEnumerable<IProject> Projects
        {
            get
            {
                return DataSet.Projects.Select(dp => _projectProxyFactory.Create(dp));
            }
        }

        /// <inheritdoc />
        public IProject CreateProject(string uniqueId, string name)
        {
            return _projectProxyFactory.Create(DataSet.Projects.AddProjectsRow(uniqueId, name, true));
        }

        public void DeleteProject(IProject project)
        {
            var xmlProject = project as XmlDataSetProjectProxy;
            if (xmlProject != null)
            {
                DataSet.Projects.RemoveProjectsRow(xmlProject.ProjectsRow);
            }
            else
            {
                throw new ArgumentException("Project is not a project from this data set.");
            }
        }

        /// <inheritdoc />
        public event ProjectsChangedEventHandler ProjectsChanged;


        /// <inheritdoc/>
        public IEnumerable<IProjectTime> Times
        {
            get
            {
                return DataSet.Times.Select(dt => _timeProxyFactory.Create(dt));
            }
        }

        /// <inheritdoc />
        public IProjectTime CreateTime(IProject project, DateTime startTime, DateTime? endTime)
        {
            var xmlProject = project as XmlDataSetProjectProxy;
            if (xmlProject != null)
            {
                return _timeProxyFactory.Create(DataSet.Times.AddTimesRow(xmlProject.ProjectsRow.ProjectID, startTime, endTime ?? XmlDataSetProjectTimeProxy.EmptyDateTime));
            }
            else
            {
                throw new ArgumentException("Project is not a project from this data set.");
            }
        }

        /// <inheritdoc />
        public event TimesChangedEventHandler ProjectTimeChanged;

        #endregion

        /// <summary>
        /// Initializes this class by reading the database from the stream.
        /// </summary>
        /// <param name="stream"></param>
        public void LoadDatabase(Stream stream)
        {
            DataSet = new TimesDataSet();

            foreach (DataTable dataTable in DataSet.Tables)
            {
                dataTable.BeginLoadData();
            }

            if (stream != null)
            {
                DataSet.ReadXml(stream);
            }

            foreach (DataTable dataTable in DataSet.Tables)
            {
                dataTable.EndLoadData();
            }

            SubscribeToEvents();
        }

        /// <summary>
        /// Stores the information in the database to the stream.
        /// </summary>
        /// <param name="stream"></param>
        public void WriteDatabase(TextWriter stream)
        {
            DataSet.WriteXml(stream);
        }

        /// <summary>
        /// We need to notify our clients of when the model changes - so we subscribe to the events from
        /// the data set so we can notify our clients.
        /// </summary>
        private void SubscribeToEvents()
        {
            if (DataSet != null)
            {
                DataSet.Projects.ProjectsRowChanged += (sender, @event) => FireProjectsChangedEvent(@event.Row);;
                DataSet.Projects.ProjectsRowDeleted += (sender, @event) => FireProjectsChangedEvent(@event.Row);

                DataSet.Times.TimesRowChanged += (sender, @event) => FireTimesChangedEvent(@event.Row);
            }
        }

        private void FireProjectsChangedEvent(TimesDataSet.ProjectsRow projectRow)
        {
            if (ProjectsChanged != null)
            {
                ProjectsChanged.Invoke(this, _projectProxyFactory.Create(projectRow));
            }
        }

        private void FireTimesChangedEvent(TimesDataSet.TimesRow timesRow)
        {
            if (ProjectTimeChanged != null)
            {
                var eventArgs = new TimesChangedEventArgs(this, _timeProxyFactory.Create(timesRow));
                ProjectTimeChanged.Invoke(this, eventArgs);
            }
        }
    }
}