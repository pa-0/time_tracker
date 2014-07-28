using Ficksworkshop.TimeTrackerAPI.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TimeTrackerAPI.Tests.Mock;

namespace TimeTrackerAPI.Tests.Commands
{
    [TestClass]
    public class CreateProjectCommandUnitTest
    {
        [TestMethod]
        [Description("If there is no data set to operate on, then the command should be disabled.")]
        public void CreateProjetCommand_CanExecute_NoDataSet_ReturnsFalse()
        {
            var command = new CreateProjectCommand(() => null);
            Assert.IsFalse(command.CanExecute(null));
        }

        [TestMethod]
        [Description("If there is a data set to operate on, then the command should be enabled.")]
        public void CreateProjetCommand_CanExecute_NoDataSet_ReturnsTrue()
        {
            var dataSet = new MemoryProjectTimesData();
            var command = new CreateProjectCommand(() => dataSet);
            Assert.IsTrue(command.CanExecute(null));
        }
    }
}
