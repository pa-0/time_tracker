using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// View model that can show a filtered list of project times.
    /// </summary>
    internal class ProjectTimesViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<IProjectTime> ProjectTimes;

        #endregion
    }
}
