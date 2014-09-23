using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTrackerAPI.Tests
{
    [TestClass]
    public class XmlDataSetProjectTimesDataUnitTest
    {
        [TestMethod]
        public void XmlDataSetProjectTimesData_CreateProject_EventIsFired()
        {
            IProjectTimesData dataInstance = new XmlDataSetProjectTimesData(null);

            // Subscribe to the event to know that it got fired
            int eventFiredCount = 0;
            dataInstance.ProjectsChanged += (sender, o) => { eventFiredCount++; };

            // If we create a project, then one should exist
            IProject project = dataInstance.CreateProject("", "");

            Assert.AreEqual(1, eventFiredCount);
        }

        [TestMethod]
        public void XmlDataSetProjectTimesData_DeleteProject_EventIsFired()
        {
            IProjectTimesData dataInstance = new XmlDataSetProjectTimesData(null);

            // If we create a project, then one should exist
            IProject project = dataInstance.CreateProject("", "");

            // Subscribe to the event to know that it got fired
            int eventFiredCount = 0;
            dataInstance.ProjectsChanged += (sender, o) => { eventFiredCount++; };

            // Delete should fire the event
            dataInstance.DeleteProject(project);

            Assert.AreEqual(1, eventFiredCount);
        }

        [TestMethod]
        public void XmlDataSetProjectTimesData_CreateProject_SetUniqueId_ProjectHasNewId()
        {
            IProjectTimesData dataInstance = new XmlDataSetProjectTimesData(null);

            // Initially, we shouldn't have any projects
            Assert.AreEqual(0, dataInstance.Projects.Count());

            // If we create a project, then one should exist
            IProject project = dataInstance.CreateProject("", "");
            Assert.AreEqual(1, dataInstance.Projects.Count());

            // Subscribe to the event to know that it got fired
            int eventFiredCount = 0;
            dataInstance.ProjectsChanged += (sender, o) => { eventFiredCount++; };

            // Change the name of the returned item. Now if we change the projects list, it should have the same name
            project.UniqueId = "Test Id";
            Assert.AreEqual("Test Id", dataInstance.Projects.First().UniqueId);
            Assert.AreEqual(1, eventFiredCount);
        }

        [TestMethod]
        public void XmlDataSetProjectTimesData_CreateProject_SetName_ProjectHasNewName()
        {
            IProjectTimesData dataInstance = new XmlDataSetProjectTimesData(null);

            // Initially, we shouldn't have any projects
            Assert.AreEqual(0, dataInstance.Projects.Count());

            // If we create a project, then one should exist
            IProject project = dataInstance.CreateProject("", "");
            Assert.AreEqual(1, dataInstance.Projects.Count());

            // Subscribe to the event since it should be fired
            int eventFiredCount = 0;
            dataInstance.ProjectsChanged += (sender, o) => { eventFiredCount++; };

            // Change the name of the returned item. Now if we change the projects list, it should have the same name
            project.Name = "Test Id";
            Assert.AreEqual("Test Id", dataInstance.Projects.First().Name);

            Assert.AreEqual(1, eventFiredCount);
        }

        [TestMethod]
        public void XmlDataSetProjectTimesData_CreateTime_EventIsFired()
        {
            IProjectTimesData dataInstance = new XmlDataSetProjectTimesData(null);

            IProject project = dataInstance.CreateProject("", "");

            // Subscribe to the event since it should be fired
            int eventFiredCount = 0;
            TimesChangedEventArgs args = null;
            dataInstance.ProjectTimeChanged +=
                (sender, eventArgs) =>
                    {
                        eventFiredCount++;
                        args = eventArgs; 
                    };

            // Create the time instance
            IProjectTime createdTime = dataInstance.CreateTime(project, DateTime.Now, null);

            Assert.IsNotNull(args);
            Assert.AreEqual(createdTime, args.ModifiedTime);
            Assert.AreEqual(dataInstance, args.DataSet);
            Assert.AreEqual(1, eventFiredCount);
        }
    }
}
