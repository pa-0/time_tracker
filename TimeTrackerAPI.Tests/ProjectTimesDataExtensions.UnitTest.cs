using System;
using Ficksworkshop.TimeTracker.TestUtilities.Mock;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ficksworkshop.TimeTrackerAPI
{
    [TestClass]
    public class ProjectTimesDataExtensionsUnitTest
    {
        [TestMethod]
        public void ProjectTimesDataExtensions_PunchedInTime_NoneOpen_ReturnsNull()
        {
            var dataSet = new MemoryProjectTimesData();
            IProject project = dataSet.CreateProject("", "");

            // Create some time entries in the project, all with ending times
            dataSet.CreateTime(project, new DateTime(2014, 06, 29), new DateTime(2014, 06, 30));
            dataSet.CreateTime(project, new DateTime(2014, 07, 30), new DateTime(2014, 07, 31));

            // Now check that we don't return any open time
            Assert.IsNull(dataSet.FirstOpenTime());
        }

        [TestMethod]
        public void ProjectTimesDataExtensions_PunchedInTime_OneOpen_ReturnsItem()
        {
            var dataSet = new MemoryProjectTimesData();
            IProject project = dataSet.CreateProject("", "");

            // Create some time entries in the project, with open time
            dataSet.CreateTime(project, new DateTime(2014, 06, 29), new DateTime(2014, 06, 30));
            IProjectTime openTime = dataSet.CreateTime(project, new DateTime(2014, 07, 30), null);

            // Now check that we don't return any open time
            Assert.AreEqual(openTime, dataSet.FirstOpenTime());
        }

        [TestMethod]
        public void ProjectTimesDataExtensions_PunchedInTimeHasProject_NoneOpen_ReturnsNull()
        {
            var dataSet = new MemoryProjectTimesData();
            IProject project = dataSet.CreateProject("", "");

            // Create some time entries in the project, all with ending times
            dataSet.CreateTime(project, new DateTime(2014, 06, 29), new DateTime(2014, 06, 30));
            dataSet.CreateTime(project, new DateTime(2014, 07, 30), new DateTime(2014, 07, 31));

            // Now check that we don't return any open time
            Assert.IsNull(dataSet.FirstOpenTime(project));
        }

        [TestMethod]
        public void ProjectTimesDataExtensions_PunchedInTimeHasProject_OneOpen_ReturnsItem()
        {
            var dataSet = new MemoryProjectTimesData();
            IProject project1 = dataSet.CreateProject("", "");
            IProject project2 = dataSet.CreateProject("", "");

            // Create some time entries in the projects some open in each project to make sure we
            // find the item from the right project
            dataSet.CreateTime(project1, new DateTime(2014, 06, 29), new DateTime(2014, 06, 30));
            dataSet.CreateTime(project1, new DateTime(2014, 07, 30), null);
            IProjectTime openTime = dataSet.CreateTime(project2, new DateTime(2014, 07, 30), null);

            // Now check that we don't return any open time
            Assert.AreEqual(openTime, dataSet.FirstOpenTime());
        }
    }
}
