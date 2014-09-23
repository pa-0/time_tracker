using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ficksworkshop.TimeTracker.Commands;
using Ficksworkshop.TimeTracker.TestUtilities.Mock;

namespace Ficksworkshop.TimeTracker.Tests.Commands
{
    [TestClass]
    public class CreateProjectCommandUnitTest
    {
        [TestMethod]
        [Description("If there is no data set to operate on, then the command should be disabled.")]
        public void CreateProjetCommand_CanExecute_NoDataSet_ReturnsFalse()
        {
            var command = new CreateProjectCommand(() => null, null, null);
            Assert.IsFalse(command.CanExecute(null));
        }

        [TestMethod]
        [Description("If there is a data set to operate on, then the command should be enabled.")]
        public void CreateProjetCommand_CanExecute_NoDataSet_ReturnsTrue()
        {
            var dataSet = new MemoryProjectTimesData();
            var command = new CreateProjectCommand(() => dataSet, null, null);
            Assert.IsTrue(command.CanExecute(null));
        }
    }
}
