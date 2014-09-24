using System;
using System.Windows.Input;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker.Commands
{
    /// <summary>
    /// A standard command to punch into to or punch out of a project. This uses the selected project manager
    /// and will only punch in or out of the project if it is the selected project.
    /// 
    /// This command required bound command pameter that specified the <see cref="IProject"/> to punch in or out of.
    /// </summary>
    public class SelectedPunchInOutCommand : ICommand
    {
        #region Fields

        private readonly SelectedProjectManager _selectedProjectManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedPunchInOutCommand" class.
        /// </summary>
        /// <param name="selectedProjectMananger"></param>
        public SelectedPunchInOutCommand(SelectedProjectManager selectedProjectMananger)
        {
            _selectedProjectManager = selectedProjectMananger;
        }

        #endregion

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            var project = parameter as IProject;
            if (project != null)
            {
                // If the project isn't selected, try to switch to it. Switching to it might fail
                // and if it fails, then we do nothing
                if (_selectedProjectManager.SelectedProject == project || _selectedProjectManager.SetSelectedProject(project))
                {
                    IProjectTime activeTimeRow = project.Owner.FirstOpenTime();
                    if (activeTimeRow != null)
                    {
                        // No open time, so do a punch in
                        activeTimeRow.End = DateTime.Now;
                    }
                    else
                    {
                        // Have an open time, so do a punch out
                        project.Owner.CreateTime(project, DateTime.Now, null);
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>We can execute if there is a context, a project, and the project isn't closed.</remarks>
        public bool CanExecute(object parameter)
        {
            var project = parameter as IProject;
            return project != null && project.Status != ProjectStatus.Closed;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
