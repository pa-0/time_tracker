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

        /// <inheritdoc />
        public IProject CreateProject()
        {
            IProject project = new XmlDataSetProjectProxy(DataSet.Projects.AddProjectsRow("Default Id", "Default Name", true));

            if (ProjectsChanged != null)
            {
                ProjectsChanged.Invoke(this, project);
            }

            return project;
        }

        public void DeleteProject(IProject project)
        {
            var xmlProject = project as XmlDataSetProjectProxy;
            if (xmlProject != null)
            {
                DataSet.Projects.RemoveProjectsRow(xmlProject.ProjectsRow);

                if (ProjectsChanged != null)
                {
                    ProjectsChanged.Invoke(this, project);
                }
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
                return DataSet.Times.Select(dt => new XmlDataSetProjectTimeProxy(DataSet, dt));
            }
        }

        /// <inheritdoc />
        public IProjectTime CreateTime(IProject project, DateTime startTime, DateTime? endTime)
        {
            var xmlProject = project as XmlDataSetProjectProxy;
            if (xmlProject != null)
            {
                return new XmlDataSetProjectTimeProxy(DataSet, DataSet.Times.AddTimesRow(xmlProject.ProjectsRow.ProjectID, startTime, endTime ?? XmlDataSetProjectTimeProxy.EmptyDateTime));
            }
            else
            {
                throw new ArgumentException("Project is not a project from this data set.");
            }
        }

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
        #region Properties

        internal TimesDataSet.ProjectsRow ProjectsRow { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSetProjectProxy"/> class.
        /// </summary>
        /// <param name="project"></param>
        public XmlDataSetProjectProxy(TimesDataSet.ProjectsRow project)
        {
            ProjectsRow = project;
        }

        #endregion

        #region IProject Members

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                return ProjectsRow.Name;
            }
            set
            {
                // TODO notify
                ProjectsRow.Name = value;
            }
        }

        /// <inheritdoc/>
        public string UniqueId
        {
            get
            {
                return ProjectsRow.Identifier;
            }
            set
            {
                // TODO notify
                ProjectsRow.Identifier = value;
            }
        }

        /// <inheritdoc/>
        public ProjectStatus Status
        {
            get
            {
                return (ProjectsRow.IsActive) ? ProjectStatus.Open : ProjectStatus.Closed;
            }
            set
            {
                ProjectsRow.IsActive = (value == ProjectStatus.Open);
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

        public static readonly DateTime EmptyDateTime = DateTime.MinValue;

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
        public DateTime? End
        {
            get
            {
                if (_timeRow.End == EmptyDateTime)
                {
                    return null;
                }
                return _timeRow.End;
            }
            set
            {
                // If we try to set to null, set to the special "null" value
                _timeRow.End = (value.HasValue) ? value.Value : EmptyDateTime;
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
