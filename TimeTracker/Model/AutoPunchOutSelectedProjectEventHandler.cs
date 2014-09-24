using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ficksworkshop.TimeTracker.Manager;

namespace Ficksworkshop.TimeTracker.Model
{
    /// <summary>
    /// In most cases, we only want to allow working on one project at a time. This listens
    /// for selected project changes, and if there is an open project time, prompts the user
    /// to either close the project time, or cancel changing the selected project.
    /// </summary>
    /// <remarks>
    /// This exists as a separate class so that we can easily disable this if desired.
    /// </remarks>
    internal class AutoPunchOutSelectedProjectEventHandler
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPunchOutSelectedProjectEventHandler"/> class.
        /// </summary>
        /// <param name="selectedProjectManager"></param>
        public AutoPunchOutSelectedProjectEventHandler(SelectedProjectManager selectedProjectManager)
        {
            selectedProjectManager.SelectedProjectChanging += SelectedProjectChanging;
        }

        #endregion

        #region Private Members

        private void SelectedProjectChanging(object sender, SelectedProjectChangingEventArgs eventArgs)
        {
            // Check if we currently have an open project time
            var openTime = TrackerInstance.DataSet.FirstOpenTime();
            if (openTime != null)
            {
                eventArgs.Cancel = true;
            }
        }

        #endregion
    }
}
