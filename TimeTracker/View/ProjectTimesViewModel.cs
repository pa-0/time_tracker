using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker.View
{
    /// <summary>
    /// View model that can show a filtered list of project times.
    /// </summary>
    // TODO Make this internal
    public class ProjectTimesViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The data set to display.
        /// </summary>
        private readonly IProjectTimesData _dataSet;

        #endregion

        #region Properties

        /// <summary>
        /// The project times that should be displayed. What times are in the list
        /// depend on the filter properties.
        /// </summary>
        public ObservableCollection<IProjectTime> ProjectTimes { get; private set; }

        private DateTime? _start = null;
        /// <summary>
        /// Filters the project times to display only after the specified start time
        /// </summary>
        public DateTime? FilterStart
        {
            get { return _start; }
            set
            {
                if (value != _start)
                {
                    _start = value;
                    NotifyPropertyChanged("FilterStart");

                    RefreshProjectTimes();
                }
            }
        }

        private DateTime? _end = null;
        /// <summary>
        /// Filters the project times to display only before the specified end time
        /// </summary>
        public DateTime? FilterEnd
        {
            get { return _end; }
            set
            {
                if (value != _end)
                {
                    _end = value;
                    NotifyPropertyChanged("FilterEnd");

                    RefreshProjectTimes();
                }
            }
        }

        /// <summary>
        /// List of projects that can be filtered on
        /// </summary>
        public ObservableCollection<IProject> FilterProjects { get; private set; }

        private IProject _filterSelectedProject = null;
        /// <summary>
        /// If not null, sets to only show times from the specified project.
        /// </summary>
        public IProject FilterSelectedProject
        {
            get { return _filterSelectedProject; }
            set
            {
                if (value != _filterSelectedProject)
                {
                    _filterSelectedProject = value;
                    NotifyPropertyChanged("FilterSelectedProject");

                    RefreshProjectTimes();
                }
            }
        }

        private TimeSpan _totalTime = new TimeSpan(0);

        /// <summary>
        /// The total time for all of the visible project times.
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return _totalTime; }
            set
            {
                if (value != _totalTime)
                {
                    _totalTime = value;
                    NotifyPropertyChanged("TotalTime");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTimesViewModel"/> class.
        /// </summary>
        /// <param name="dataSet">The data set to show information for.</param>
        public ProjectTimesViewModel(IProjectTimesData dataSet)
        {
            _dataSet = dataSet;

            ProjectTimes = new ObservableCollection<IProjectTime>();

            FilterProjects = new ObservableCollection<IProject>();

            // Populate the list of projects we can filter on
            ProjectsChanged(null, null);
            // TODO memory leak here since we never unlisten
            dataSet.ProjectsChanged += ProjectsChanged;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Event handler called when the list of projects changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectsChanged(object sender, object e)
        {
            // TODO I'm being lazy here and just removing and adding. Once there is a way to know
            // which projects were added/removed, then I can more inteligently add/remove from
            // the collection

            FilterProjects.Clear();
            foreach (var project in _dataSet.Projects)
            {
                FilterProjects.Add(project);
            }
            
            // If we are filtering on a project and the project isn't currently in the list, then stop
            // filtering on that project.
            if (FilterSelectedProject != null && !FilterProjects.Contains(FilterSelectedProject))
            {
                FilterSelectedProject = null;
            }
        }

        private void RefreshProjectTimes()
        {
            // Repopulate the project times list according to the filter settings
            ProjectTimes.Clear();

            IEnumerable<IProjectTime> times = _dataSet.Times;

            // Add the filter by time
            if (FilterStart != null || FilterEnd != null)
            {
                DateTime start = (FilterStart.HasValue) ? FilterStart.Value : DateTime.MinValue;
                DateTime end = (FilterEnd.HasValue) ? FilterEnd.Value : DateTime.MaxValue;
                times = times.Where(pt => pt.Start > start && pt.End.HasValue && pt.End < end);
            }
            
            // Add the filter by project only if we have any project selected
            if (FilterSelectedProject != null)
            {
                times = times.Where(pt => FilterProjects.Contains(pt.Project));
            }

            foreach (var projectTime in times)
            {
                ProjectTimes.Add(projectTime);
            }

            // Calculate the total time and set the property so we can easily see it
            TotalTime = ProjectTimes.SumTimeSpan();
        }

        #endregion
    }
}
