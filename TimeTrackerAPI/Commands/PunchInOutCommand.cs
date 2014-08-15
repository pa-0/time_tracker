using System;
using System.Windows.Input;

namespace Ficksworkshop.TimeTrackerAPI.Commands
{
    /// <summary>
    /// A standard command to punch into to or punch out of a project. If not punched in, this acts as a punch in,
    /// otherwise if punched in, this acts as a punch out.
    /// 
    /// This command required bound command pameter that specified the <see cref="IProject"/> to punch in or out of.
    /// </summary>
    public class PunchInOutCommand : ICommand
    {        
        /// <inheritdoc />
        public void Execute(object parameter)
        {
            var project = parameter as IProject;
            if (project != null)
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
