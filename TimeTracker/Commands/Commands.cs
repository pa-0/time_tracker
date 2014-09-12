using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTrackerAPI.Commands;

namespace Ficksworkshop.TimeTracker.Commands
{
    /// <summary>
    /// Application commands (that generally are not context specific). They are always valid.
    /// </summary>
    public static class Commands
    {
        /// <summary>
        /// Command to shutdown the application.
        /// </summary>
        public static readonly ICommand ExitApplicationCommand = new DelegateCommand
            {
                CommandAction = () =>
                {
                    // Get the current path
                    StringSetting pathSetting = (StringSetting)TrackerInstance.Settings.Items.Where(i => i.Name == TrackerSettings.LastDataSet).First();


                    XmlDataSetProjectTimesData xmlDataSet = (XmlDataSetProjectTimesData) TrackerInstance.DataSet;
                    TextWriter writer = new StreamWriter(new FileStream(pathSetting.Value, FileMode.OpenOrCreate));
                    xmlDataSet.WriteDatabase(writer);
                    Application.Current.Shutdown();
                }
            };

        /// <summary>
        /// Command to show the application UI (if it was not visible).
        /// </summary>
        public static readonly ICommand ShowWindowCommand = new DelegateCommand
            {
                CanExecuteFunc = () => Application.Current.MainWindow == null,
                CommandAction = () =>
                    {
                        Application.Current.MainWindow = new MainWindow();
                        var viewModel = new MainWindowViewModel(TrackerInstance.Settings, TrackerInstance.DataSet);
                        Application.Current.MainWindow.DataContext = viewModel;
                        Application.Current.MainWindow.Show();
                    }
            };

        /// <summary>
        /// Show a dialog that can summaries information about project times.
        /// </summary>
        public static readonly ICommand ViewDayDetailsCommand = new DelegateCommand
            {
                CanExecuteFunc = () => TrackerInstance.DataSet != null && Application.Current.MainWindow == null,
                CommandAction = () =>
                {
                    IProjectTimesData dataSet = TrackerInstance.DataSet;

                    Application.Current.MainWindow = new ProjectTimesWindow();
                    var viewModel = new ProjectTimesViewModel(TrackerInstance.DataSet);
                    Application.Current.MainWindow.DataContext = viewModel;
                    Application.Current.MainWindow.Show();
                }
            };

        /// <summary>
        /// Load or change the current data set. Currently, this just picks a default path.
        /// </summary>
        public static readonly ICommand LoadDataSetCommand = new DelegateCommand
        {
            CommandAction = () =>
            {
                MessageBoxResult result = MessageBoxResult.Yes;

                // Check if we have an open data set that we might want to modify. Right now there isn't a dirty mark, so
                // I'm doing a simple check of any projects. If there are projects, then assume there is data that we might
                // want to save
                if (TrackerInstance.DataSet.Projects.Any())
                {
                    result = MessageBox.Show(Resources.SaveTrackerData, "", MessageBoxButton.YesNo);
                }

                // Ask for the file that we want to open
                if (result == MessageBoxResult.Yes)
                {
                    var openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog().Value)
                    {
                        // If we are ok, then open the new tracker data set
                        try
                        {
                            TrackerInstance.OpenDataSet(openFileDialog.FileName);

                            // Since we were successful, set the new path as our last path
                            TrackerInstance.Settings.Items.Where(i => i.Name == TrackerSettings.LastDataSet).First().Value = openFileDialog.FileName;
                        }
                        catch (Exception)
                        {
                            // TODO catching all is bad!
                        }
                    }
                }
            }
        };

        // TODO hack hack hack
        public static readonly ICommand CreateNewProjectCommand = new CreateProjectCommand(() => TrackerInstance.DataSet, null, null);

        public static readonly ICommand DeleteProjectCommand = new DeleteProjectCommand();

        public static readonly ICommand PunchInOutCommand = new PunchInOutCommand();
    }

    /// <summary>
    /// Simplistic delegate command.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
