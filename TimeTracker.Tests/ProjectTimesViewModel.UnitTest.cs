using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ficksworkshop.TimeTracker;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTracker.Model;
using Ficksworkshop.TimeTracker.TestUtilities.Mock;
using Ficksworkshop.TimeTracker.View;

namespace TimeTracker.Tests
{
    [TestClass]
    public class ProjectTimesViewModelUnitTest
    {
        [TestMethod]
        public void ProjectTimesViewModel_NoFilter_AllItemsInFilteredList()
        {
            IProjectTimesData projectTimes = CreateProjectTimes();

            var vm = new ProjectTimesViewModel(projectTimes);

            CollectionAssert.AreEquivalent(projectTimes.Projects.ToList(), vm.FilterProjects);
        }

        private IProjectTimesData CreateProjectTimes()
        {
            var projectTimes = new MemoryProjectTimesData();

            var projectOpen = projectTimes.CreateProject("1", "Project1");
            projectTimes.CreateTime(projectOpen, new DateTime(), new DateTime());
            projectTimes.CreateTime(projectOpen, new DateTime(), new DateTime());

            var projectClosed = projectTimes.CreateProject("2", "Project2");
            projectTimes.CreateTime(projectOpen, new DateTime(), new DateTime());
            projectClosed.Status = ProjectStatus.Closed;

            return projectTimes;
        }
    }
}
