using System;
using System.Collections.Generic;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker.TestUtilities.Mock
{
    /// <summary>
    /// An implementation of the <see cref="IProjectTimesData"/> that exists only in memory
    /// (and used by testing)
    /// </summary>
    public class MemoryProjectTimesData : IProjectTimesData
    {
        private IList<MemoryProject> _projects = new List<MemoryProject>();

        private IList<MemoryTime> _times = new List<MemoryTime>();

        #region IProjectTimesData Members

        /// <inheritdoc />
        public IProject CreateProject(string uniqueId, string name)
        {
            var newProject = new MemoryProject(this);
            _projects.Add(newProject);
            return newProject;
        }

        /// <inheritdoc />
        public void DeleteProject(IProject project)
        {
            _projects.Remove((MemoryProject)project);
        }

        /// <inheritdoc />
        public IEnumerable<IProject> Projects
        {
            get
            {
                foreach (var project in _projects)
                {
                    yield return project;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<IProjectTime> Times
        {
            get
            {
                foreach (var time in _times)
                {
                    yield return time;
                }
            }
        }

        /// <inheritdoc />
        public IProjectTime CreateTime(IProject project, DateTime start, DateTime? end)
        {
            return null;
        }

        /// <inheritdoc />
        public event ProjectsChangedEventHandler ProjectsChanged;

        /// <inheritdoc />
        public event TimesChangedEventHandler ProjectTimeChanged;

        #endregion

        #region Nested Types

        internal class MemoryProject : IProject
        {
            public MemoryProject(MemoryProjectTimesData dataSet)
            {
                Owner = dataSet;
            }

            public string Name { get; set; }

            public ProjectStatus Status { get; set; }

            public string UniqueId { get; set; }

            public IProjectTimesData Owner { get; private set; }
        }

        internal class MemoryTime : IProjectTime
        {
            internal MemoryProject ActualProject { get; set; }

            public DateTime Start { get; set; }

            public DateTime? End { get; set; }

            public IProject Project
            {
                get
                {
                    return ActualProject;
                }
                set
                {
                    ActualProject = (MemoryProject)value;
                }
            }
        }

        #endregion
    }
}
