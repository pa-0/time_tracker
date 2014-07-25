using System;
using System.Collections.Generic;

namespace Ficksworkshop.TimeTrackerAPI
{
    public delegate void ProjectsChangedEventHandler(object sender, object e);

    /// <summary>
    /// Represents the view of project times data that is exposed externally. The purpose of these
    /// interfaces is to allow chaning out the core data storage without affecting clients of the API.
    /// </summary>
    public interface IProjectTimesData
    {
        /// <summary>
        /// Gets a listing of all projects.
        /// </summary>
        IEnumerable<IProject> Projects { get; }

        /// <summary>
        /// Gets a listing of all times.
        /// </summary>
        IEnumerable<IProjectTime> Times { get; }

        /// <summary>
        /// Creates a new project instance.
        /// </summary>
        /// <returns>The new project instance.</returns>
        IProject CreateProject();

        event ProjectsChangedEventHandler ProjectsChanged;
    }

    public enum ProjectStatus
    {
        Open,
        Closed,
    }

    public interface IProject
    {
        string Name { get; set; }

        string UniqueId { get; set; }

        ProjectStatus Status { get; set; }
    }

    public interface IProjectTime
    {
        DateTime Start { get; set; }

        DateTime End { get; set; }

        IProject Project { get; }
    }
}
