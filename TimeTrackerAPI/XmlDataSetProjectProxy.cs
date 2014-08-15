using System.Data;

namespace Ficksworkshop.TimeTrackerAPI
{
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
        /// <param name="owner"></param>
        /// <param name="project"></param>
        public XmlDataSetProjectProxy(XmlDataSetProjectTimesData owner, TimesDataSet.ProjectsRow project)
        {
            ProjectsRow = project;
            _owner = owner;
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

        private readonly XmlDataSetProjectTimesData _owner;

        public IProjectTimesData Owner
        {
            get { return _owner; }
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Factory that generates <see cref="XmlDataSetProjectProxy"/>, and returning the same item if already generated.
    /// </summary>
    internal class XmlDataSetProjectProxyFactory : XmlDataSetProxyFactory<TimesDataSet.ProjectsRow, XmlDataSetProjectProxy>
    {
        #region Fields

        private readonly XmlDataSetProjectTimesData _dataSet;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSetProjectProxyFactory"/> class.
        /// </summary>
        /// <param name="dataSet">The owner of the project items.</param>
        /// <param name="table">The table that contains the rows.</param>
        internal XmlDataSetProjectProxyFactory(XmlDataSetProjectTimesData dataSet, TypedTableBase<TimesDataSet.ProjectsRow> table)
            : base(table)
        {
            _dataSet = dataSet;
        }

        /// <inheritdoc/>
        protected override XmlDataSetProjectProxy CreateInstance(TimesDataSet.ProjectsRow row)
        {
            return new XmlDataSetProjectProxy(_dataSet, row);
        }
    }
}
