using System;
using System.Windows.Input;

namespace Ficksworkshop.TimeTrackerAPI.Commands
{
    /// <summary>
    /// A standard command that can delete a project.
    /// 
    /// This command required bound command pameter that specified the <see cref="IProject"/> to delete.
    /// </summary>
    public class DeleteProjectCommand : ICommand
    {
        /// <inheritdoc />
        public void Execute(object parameter)
        {
            var project = parameter as IProject;
            if (project != null)
            {
                project.Owner.DeleteProject(project);
            }
        }

        /// <inheritdoc />
        /// <remarks>We can execute if there is a context and a project.</remarks>
        public bool CanExecute(object parameter)
        {
            var project = parameter as IProject;
            return project != null;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
