using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ficksworkshop.TimeTrackerAPI.Tests
{
    [TestClass]
    public class XmlDataSetProjectTimesDataUnitTest
    {
        [TestMethod]
        public void XmlDataSetProjectTimesData_CreateProject_SetUniqueId_ProjectHasNewId()
        {
            IProjectTimesData dataInstance = new XmlDataSetProjectTimesData(null);

            // Initially, we shouldn't have any projects
            Assert.AreEqual(0, dataInstance.Projects.Count());

            // If we create a project, then one should exist
            IProject project = dataInstance.CreateProject();
            Assert.AreEqual(1, dataInstance.Projects.Count());

            // What is the default identifier of the project
            string defaultId = project.UniqueId;
            
            // Change the name of the returned item. Now if we change the projects list, it should have the same name
            project.UniqueId = "Test Id";
            Assert.AreEqual("Test Id", dataInstance.Projects.First().UniqueId);

            if (defaultId == "Test Id")
            {
                Assert.Inconclusive("We want to change the ID, but the ID is already the ID we would use, so can't tell if it was successful");
            }
        }

        [TestMethod]
        public void XmlDataSetProjectTimesData_CreateProject_SetName_ProjectHasNewName()
        {
            IProjectTimesData dataInstance = new XmlDataSetProjectTimesData(null);

            // Initially, we shouldn't have any projects
            Assert.AreEqual(0, dataInstance.Projects.Count());

            // If we create a project, then one should exist
            IProject project = dataInstance.CreateProject();
            Assert.AreEqual(1, dataInstance.Projects.Count());

            // What is the default identifier of the project
            string defaultName = project.Name;

            // Change the name of the returned item. Now if we change the projects list, it should have the same name
            project.Name = "Test Id";
            Assert.AreEqual("Test Id", dataInstance.Projects.First().Name);

            if (defaultName == "Test Id")
            {
                Assert.Inconclusive("We want to change the name, but the name is already the name we would use, so can't tell if it was successful");
            }
        }
    }
}
