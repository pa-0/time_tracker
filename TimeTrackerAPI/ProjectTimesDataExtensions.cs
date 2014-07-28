using System;
using System.Linq;

namespace Ficksworkshop.TimeTrackerAPI
{
    /// <summary>
    /// Extension methos for <see cref="IProjectTimesData"/>. These extension methods should work on any
    /// implementation of the <see cref="IProjectTimesData"/> and exist so that they can be shared among
    /// different implementations.
    /// </summary>
    public static class ProjectTimesDataExtensions
    {
        /// <summary>
        /// Gets the currently punched in project time, or null if not currently punched in.
        /// </summary>
        /// <returns>The time item, or null if not punched in.</returns>
        public static IProjectTime PunchedInTime(this IProjectTimesData dataSet)
        {
            return dataSet.Times.FirstOrDefault(timeRow => timeRow.End == default(DateTime));
        }
    }
}
