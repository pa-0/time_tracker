using System;
using System.Windows;
using System.Windows.Input;
using Ficksworkshop.TimeTrackerAPI;
using Ficksworkshop.TimeTrackerAPI.Commands;

namespace Ficksworkshop.TimeTracker
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
                CommandAction = () => Application.Current.Shutdown()
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

        public static readonly ICommand CreateNewProjectCommand = new CreateProjectCommand(null, null);

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
