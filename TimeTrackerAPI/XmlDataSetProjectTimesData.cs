using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSetProjectTimesData"/> class.
        /// </summary>
        /// <param name="inputStream">Input stream to read XML data from. May be null.</param>
        public XmlDataSetProjectTimesData(Stream inputStream)
        {
            LoadDatabase(inputStream);
        }

        #endregion

        #region IProjectTimesData Members

        /// <inheritdoc/>
        public IEnumerable<IProject> Projects
        {
            get
            {
                return DataSet.Projects.Select(dp => new XmlDataSetProjectProxy(dp));
            }
        }

        /// <inheritdoc/>
        public IEnumerable<IProjectTime> Times
        {
            get
            {
                return DataSet.Times.Select(dt => new XmlDataSetProjectTimeProxy(DataSet, dt));
            }
        }

        /// <inheritdoc />
        public IProject CreateProject()
        {
            IProject project =  new XmlDataSetProjectProxy(DataSet.Projects.AddProjectsRow("Default Id", "Default Name", false));

            ProjectsChanged.Invoke(this, project);

            return project;
        }

        /// <inheritdoc />
        public event ProjectsChangedEventHandler ProjectsChanged;

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
        }

        /// <summary>
        /// Stores the information in the database to the stream.
        /// </summary>
        /// <param name="stream"></param>
        public void WriteDatabase(TextWriter stream)
        {
            DataSet.WriteXml(stream);
        }

    }

    /// <summary>
    /// Envoy for the <see cref="TimesDataSet.ProjectsRow"/> to represent as an <see cref="IProject"/>.
    /// </summary>
    internal class XmlDataSetProjectProxy : IProject
    {
        #region Fields

        private readonly TimesDataSet.ProjectsRow _projectRow;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSetProjectProxy"/> class.
        /// </summary>
        /// <param name="project"></param>
        public XmlDataSetProjectProxy(TimesDataSet.ProjectsRow project)
        {
            _projectRow = project;
        }

        #endregion

        #region IProject Members

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                return _projectRow.Name;
            }
            set
            {
                // TODO notify
                _projectRow.Name = value;
            }
        }

        /// <inheritdoc/>
        public string UniqueId
        {
            get
            {
                return _projectRow.Identifier;
            }
            set
            {
                // TODO notify
                _projectRow.Identifier = value;
            }
        }

        /// <inheritdoc/>
        public ProjectStatus Status
        {
            get
            {
                return (_projectRow.IsActive) ? ProjectStatus.Open : ProjectStatus.Closed;
            }
            set
            {
                _projectRow.IsActive = (value == ProjectStatus.Open);
            }
        }

        #endregion
    }

    /// <summary>
    /// Envoy for the <see cref="TimesDataSet.TimesRow"/> to represent as an <see cref="IProjectTime"/>.
    /// </summary>
    internal class XmlDataSetProjectTimeProxy : IProjectTime
    {
        #region Fields

        private readonly TimesDataSet.TimesRow _timeRow;

        // TODO this is needed to return the project, but it is stupid to have it
        private readonly TimesDataSet _dataSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSetProjectTimeProxy"/> class.
        /// </summary>
        /// <param name="dataSet">The data set that owns the time information,</param>
        /// <param name="timeRow">The time row.</param>
        internal XmlDataSetProjectTimeProxy(TimesDataSet dataSet, TimesDataSet.TimesRow timeRow)
        {
            _dataSet = dataSet;
            _timeRow = timeRow;
        }

        #endregion

        #region IProjectTime Members

        /// <inheritdoc/>
        public DateTime Start
        {
            get
            {
                return _timeRow.Start;
            }
            set
            {
                _timeRow.Start = value;
            }
        }

        /// <inheritdoc/>
        public DateTime End
        {
            get
            {
                return _timeRow.End;
            }
            set
            {
                _timeRow.End = value;
            }
        }

        /// <inheritdoc/>
        public IProject Project
        {
            get
            {
                return new XmlDataSetProjectProxy(_dataSet.Projects.First(dp => dp.ProjectID == _timeRow.ProjectID));
            }
        }

        #endregion
    }
}
