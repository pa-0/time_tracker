using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Ficksworkshop.TimeTrackerAPI;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// View model that can show a filtered list of project times.
    /// </summary>
    // TODO Make this internal
    public class ProjectTimesViewModel : ViewModelBase
    {
        #region Fields

        private readonly IProjectTimesData _dataSet;

        #endregion

        #region Properties

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
        /// Filters the project times to display only times from the selected projects
        /// </summary>
        public ObservableCollection<IProject> FilterProjects { get; private set; }

        #endregion

        #region Constructors

        public ProjectTimesViewModel(IProjectTimesData dataSet)
        {
            _dataSet = dataSet;

            ProjectTimes = new ObservableCollection<IProjectTime>();

            FilterProjects = new ObservableCollection<IProject>();
            FilterProjects.CollectionChanged += FilterProjectsOnCollectionChanged;

            // Populate the list of projects we can filter on
            ProjectsChanged(null, null);
            // TODO memory leak here since we never unlisten
            dataSet.ProjectsChanged += ProjectsChanged;
        }

        void ProjectsChanged(object sender, object e)
        {
            // TODO I'm being lazy here and just removing and adding. Once there is a way to know
            // which projects were added/removed, then I can more inteligently add/remove from
            // the collection

            FilterProjects.Clear();
            foreach(var project in _dataSet.Projects)
            {
                FilterProjects.Add(project);
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Event handler for when the filter list changes so we can update the filtered projects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notifyCollectionChangedEventArgs"></param>
        private void FilterProjectsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            RefreshProjectTimes();
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
            if (FilterProjects.Count > 0)
            {
                times = times.Where(pt => FilterProjects.Contains(pt.Project));
            }

            foreach (var projectTime in times)
            {
                ProjectTimes.Add(projectTime);
            }
        }

        #endregion
    }
}
