using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTrackerAPI
{
    [TestClass]
    public class ProjectTimeExtensionTestClass
    {
        [TestMethod]
        public void ProjectTimeExtension_ToSortedProjectTimes_SortsByProject()
        {
            // Create two projects
            var project1 = new Mock<IProject>();
            var project2 = new Mock<IProject>();

            // Add 2 times into project1
            var time1 = new Mock<IProjectTime>();
            time1.Setup(t => t.Project).Returns(project1.Object);
            var time2 = new Mock<IProjectTime>();
            time2.Setup(t => t.Project).Returns(project1.Object);

            // Add 1 time into project2
            var time3 = new Mock<IProjectTime>();
            time3.Setup(t => t.Project).Returns(project2.Object);

            var projectTimes = new List<IProjectTime> { time1.Object, time2.Object, time3.Object };

            // Now do the sort
            IEnumerable<ProjectTimesGroup> sortedProjects = projectTimes.ToSortedProjectTimes();

            // Should have 2 groups
            Assert.AreEqual(2, sortedProjects.Count());

            // Find the first group
            var groupForProject1 = sortedProjects.First(sp => sp.Project == project1.Object);
            // And find the two items in it
            Assert.AreEqual(2, groupForProject1.ProjectTimes.Count);
            Assert.IsTrue(groupForProject1.ProjectTimes.Contains(time1.Object));
            Assert.IsTrue(groupForProject1.ProjectTimes.Contains(time2.Object));

            var groupForProject2 = sortedProjects.First(sp => sp.Project == project2.Object);
            // And find the two items in it
            Assert.AreEqual(1, groupForProject2.ProjectTimes.Count);
            Assert.IsTrue(groupForProject2.ProjectTimes.Contains(time3.Object));
        }
    }
}