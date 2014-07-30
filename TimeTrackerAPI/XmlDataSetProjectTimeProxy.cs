using System;
using System.Linq;

namespace Ficksworkshop.TimeTrackerAPI
{
    /// <summary>
    /// Envoy for the <see cref="TimesDataSet.TimesRow"/> to represent as an <see cref="IProjectTime"/>.
    /// </summary>
    internal class XmlDataSetProjectTimeProxy : IProjectTime
    {
        #region Fields

        private readonly TimesDataSet.TimesRow _timeRow;

        // TODO this is needed to return the project, but it is stupid to have it
        private readonly XmlDataSetProjectTimesData _dataSet;

        public static readonly DateTime EmptyDateTime = DateTime.MinValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSetProjectTimeProxy"/> class.
        /// </summary>
        /// <param name="dataSet">The data set that owns the time information,</param>
        /// <param name="timeRow">The time row.</param>
        internal XmlDataSetProjectTimeProxy(XmlDataSetProjectTimesData dataSet, TimesDataSet.TimesRow timeRow)
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
                // TODO this is wrong - it needs to use 
                return _dataSet.Projects.First(dp => ((XmlDataSetProjectProxy)dp).ProjectsRow.ProjectID == _timeRow.ProjectID);
            }
        }

        #endregion
    }

    /// <summary>
    /// Factory that generates <see cref="XmlDataSetProjectProxy"/>, and returning the same item if already generated.
    /// </summary>
    internal class XmlDataSetProjectTimeProxyFactory : XmlDataSetProxyFactory<TimesDataSet.TimesRow, XmlDataSetProjectTimeProxy>
    {
        private readonly XmlDataSetProjectTimesData _dataSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSetProjectTimeProxyFactory"/> class.
        /// </summary>
        /// <param name="dataSet">The data set this generates time proxies for.</param>
        internal XmlDataSetProjectTimeProxyFactory(XmlDataSetProjectTimesData dataSet)
        {
            _dataSet = dataSet;
        }

        /// <inheritdoc/>
        protected override XmlDataSetProjectTimeProxy CreateInstance(TimesDataSet.TimesRow row)
        {
            return new XmlDataSetProjectTimeProxy(_dataSet, row);
        }
    }
}
