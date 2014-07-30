using System;
using System.Collections.Generic;
using System.Linq;

namespace Ficksworkshop.TimeTrackerAPI
{
    /// <summary>
    /// Represents a group of related times for the same project.
    /// </summary>
    public class ProjectTimesGroup
    {
        public IProject Project { get; private set; }

        public IList<IProjectTime> ProjectTimes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTimesGroup"/> class.
        /// </summary>
        /// <param name="project">The project this represents.</param>
        internal ProjectTimesGroup(IProject project)
        {
            Project = project;
            ProjectTimes = new List<IProjectTime>();
        }
    }

    public static class ProjectTimeExtensions
    {
        /// <summary>
        /// Helper function to get the times that are in the specified range. This rejects any times that are open.
        /// </summary>
        /// <param name="times">The times to query.</param>
        /// <param name="start">The start time.</param>
        /// <param name="end">The end time.</param>
        /// <returns>Time entries between the range.</returns>
        public static IEnumerable<IProjectTime> TimesInRange(this IEnumerable<IProjectTime> times, DateTime start, DateTime end)
        {
            return times.Where(item => item.Start > start && item.End.HasValue && item.End.Value < end);
        }

        /// <summary>
        /// Converts the list of project times into a sorted set of project times, organized by project.
        /// </summary>
        /// <param name="times">The times to convert.</param>
        /// <returns>The sorted set.</returns>
        public static IEnumerable<ProjectTimesGroup> ToSortedProjectTimes(this IEnumerable<IProjectTime> times)
        {
            var sortedProjectTimes = new List<ProjectTimesGroup>();

            foreach (var projectTime in times)
            {
                ProjectTimesGroup existingGroup = sortedProjectTimes.Find(tg => tg.Project == projectTime.Project);

                // If the group doesn't exist yet, create it
                if (existingGroup == null)
                {
                    existingGroup = new ProjectTimesGroup(projectTime.Project);
                    sortedProjectTimes.Add(existingGroup);
                }

                // Now add to the list
                existingGroup.ProjectTimes.Add(projectTime);
            }

            return sortedProjectTimes;
        }
    }
}