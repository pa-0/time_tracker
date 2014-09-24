using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ficksworkshop.TimeTracker.Model;
using Ficksworkshop.TimeTracker.Commands;

namespace Ficksworkshop.TimeTracker.Tests.Commands
{
    [TestClass]
    public class SelectedPunchInOutCommandUnitTest
    {
        [TestMethod]
        [Description("Try to punch into a project, but fail when switching to it, so do nothing.")]
        public void SelectedPunchInOutCommand_Execute_SetSelectedProjectCanceled_DoesntPunchInOut()
        {
            var selectedProjectManager = new SelectedProjectManager();
            var owner = new Mock<IProjectTimesData>();
            var currentProject = new Mock<IProject>();
            currentProject.Setup(cp => cp.Owner).Returns(owner.Object);
            var desiredProjet = new Mock<IProject>();
            currentProject.Setup(cp => cp.Owner).Returns(owner.Object);

            // Set the selected project as our initial state before setting up the cancel all
            selectedProjectManager.SetSelectedProject(currentProject.Object);

            // Always cancel
            selectedProjectManager.SelectedProjectChanging += (sender, eventArgs) => { eventArgs.Cancel = true; };

            // Now create our command
            var command = new SelectedPunchInOutCommand(selectedProjectManager);

            // Try to execute the command
            command.Execute(desiredProjet.Object);

            owner.Verify(o => o.CreateTime(It.IsAny<IProject>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()), Times.Never());
        }

        [TestMethod]
        [Description("Try to punch into a project, when that project isn't our selected project.")]
        public void SelectedPunchInOutCommand_Execute_SetSelectedProjectChangesProject_CreatesTime()
        {
            var selectedProjectManager = new SelectedProjectManager();
            var owner = new Mock<IProjectTimesData>();
            owner.Setup(o => o.Times).Returns(Enumerable.Empty<IProjectTime>());
            var currentProject = new Mock<IProject>();
            currentProject.Setup(cp => cp.Owner).Returns(owner.Object);
            var desiredProject = new Mock<IProject>();
            desiredProject.Setup(cp => cp.Owner).Returns(owner.Object);

            // Set the selected project as our initial state before setting up the cancel all
            selectedProjectManager.SetSelectedProject(currentProject.Object);

            // Now create our command
            var command = new SelectedPunchInOutCommand(selectedProjectManager);

            // Try to execute the command
            command.Execute(desiredProject.Object);

            owner.Verify(o => o.CreateTime(desiredProject.Object, It.IsAny<DateTime>(), It.IsAny<DateTime?>()), Times.Once());
        }
    }
}
