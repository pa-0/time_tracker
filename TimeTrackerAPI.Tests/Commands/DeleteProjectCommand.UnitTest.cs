using System.Linq;
using Ficksworkshop.TimeTrackerAPI.Commands;
using Ficksworkshop.TimeTrackerAPI.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ficksworkshop.TimeTrackerAPI.Tests.Commands
{
    [TestClass]
    public class DeleteProjectCommandUnitTest
    {
        [TestMethod]
        [Description("If there is no data set to operate on, then the command should be disabled.")]
        public void DeleteProjetCommand_CanExecute_NoDataSet_ReturnsFalse()
        {
            var command = new DeleteProjectCommand(() => null, () => null);
            Assert.IsFalse(command.CanExecute(null));
        }

        [TestMethod]
        [Description("If there is no data set to operate on, then the command should be disabled.")]
        public void DeleteProjetCommand_CanExecute_NoProject_ReturnsFalse()
        {
            var dataSet = new MemoryProjectTimesData();
            var command = new DeleteProjectCommand(() => dataSet, () => null);
            Assert.IsFalse(command.CanExecute(null));
        }

        [TestMethod]
        [Description("If there is a data set and project, then command should be enabled.")]
        public void DeleteProjetCommand_CanExecute_HasProject_ReturnsTrue()
        {
            var dataSet = new MemoryProjectTimesData();
            IProject project = dataSet.CreateProject("", "");
            var command = new DeleteProjectCommand(() => dataSet, () => project);
            Assert.IsTrue(command.CanExecute(null));
        }

        [TestMethod]
        [Description("If there is a data set and project, then command should be enabled.")]
        public void DeleteProjetCommand_Execute_HasProject_DeletedProject()
        {
            var dataSet = new MemoryProjectTimesData();
            IProject project = dataSet.CreateProject("", "");
            var command = new DeleteProjectCommand(() => dataSet, () => project);
            
            command.Execute(null);

            // Now should not have any projects
            Assert.IsFalse(dataSet.Projects.Any());
        }
    }
}
