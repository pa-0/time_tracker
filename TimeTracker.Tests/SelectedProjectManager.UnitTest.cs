using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker.Tests
{
    [TestClass]
    public class SelectedProjectManagerUnitTest
    {
        [TestMethod]
        public void SelectedProjectManager_SetSelectedProject_NOListeners_ChangedProject()
        {
            var selectedProjectManager = new SelectedProjectManager();
            var newProject = new Mock<IProject>();

            // Should return false meaning not changed
            Assert.IsTrue(selectedProjectManager.SetSelectedProject(newProject.Object));

            // Didn't actually change the project
            Assert.AreEqual(newProject.Object, selectedProjectManager.SelectedProject);
        }

        [TestMethod]
        public void SelectedProjectManager_SetSelectedProject_Canceled_DoesntFireEvent()
        {
            var selectedProjectManager = new SelectedProjectManager();
            var newProject = new Mock<IProject>();

            // Always cancel
            bool changingCalled = false;
            selectedProjectManager.SelectedProjectChanging += (sender, eventArgs) => { eventArgs.Cancel = true; changingCalled = true; };

            // Add listerner to check that we don't fire that changed event, since it wasn't changed
            bool changedCalled = false;
            selectedProjectManager.SelectedProjectChanged += (sender, eventArgs) => { changedCalled = true; };

            // Should return false meaning not changed
            Assert.IsFalse(selectedProjectManager.SetSelectedProject(newProject.Object));

            // Correctly called the events
            Assert.IsTrue(changingCalled);
            Assert.IsFalse(changedCalled);

            // Didn't actually change the project
            Assert.IsNull(selectedProjectManager.SelectedProject);
        }


    }
}
