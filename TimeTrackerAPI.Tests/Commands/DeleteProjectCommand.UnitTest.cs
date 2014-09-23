using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ficksworkshop.TimeTracker.Commands;
using Ficksworkshop.TimeTracker.Model;
using Ficksworkshop.TimeTracker.TestUtilities.Mock;

namespace Ficksworkshop.TimeTrackerAPI.Tests.Commands
{
    [TestClass]
    public class DeleteProjectCommandUnitTest
    {
        [TestMethod]
        [Description("If there is no data set to operate on, then the command should be disabled.")]
        public void DeleteProjetCommand_CanExecute_NoProject_ReturnsFalse()
        {
            var command = new DeleteProjectCommand();
            Assert.IsFalse(command.CanExecute(null));
        }

        [TestMethod]
        [Description("If there is a data set and project, then command should be enabled.")]
        public void DeleteProjetCommand_CanExecute_HasProject_ReturnsTrue()
        {
            var dataSet = new MemoryProjectTimesData();
            IProject project = dataSet.CreateProject("", "");
            var command = new DeleteProjectCommand();
            Assert.IsTrue(command.CanExecute(project));
        }

        [TestMethod]
        [Description("If there is a data set and project, then command should be enabled.")]
        public void DeleteProjetCommand_Execute_HasProject_DeletesProject()
        {
            var dataSet = new MemoryProjectTimesData();
            IProject project = dataSet.CreateProject("", "");
            var command = new DeleteProjectCommand();
            command.Execute(project);

            // Now should not have any projects
            Assert.IsFalse(dataSet.Projects.Any());
        }
    }
}
