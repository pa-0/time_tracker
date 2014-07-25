using System;
using System.Windows.Input;

namespace Ficksworkshop.TimeTrackerAPI.Commands
{
    /// <summary>
    /// A standard command that can delete a project.
    /// </summary>
    public class DeleteProjectCommand : ICommand
    {
        #region Fields

        private readonly Func<IProjectTimesData> _commandContextFunc;
        private readonly Func<IProject> _projectFunc;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProjectCommand"/> class.
        /// </summary>
        /// <param name="commandContext">A function that returns an <see cref="IProjectTimesData"/>
        /// that should be used to execute command. This function may return null.</param>
        /// <param name="project">a function that returns the project that should be deleted.
        /// This function may return null to indicate there is no project available to delete.</param>
        public DeleteProjectCommand(Func<IProjectTimesData> commandContext, Func<IProject> project)
        {
            _commandContextFunc = commandContext;
            _projectFunc = project;
        }

        #endregion

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            IProjectTimesData dataSet = _commandContextFunc.Invoke();
            IProject project = _projectFunc.Invoke();
            if (dataSet != null && project != null)
            {
                dataSet.DeleteProject(project);
            }
        }

        /// <inheritdoc />
        /// <remarks>We can execute if there is a context and a project.</remarks>
        public bool CanExecute(object parameter)
        {
            return _commandContextFunc.Invoke() != null && _projectFunc.Invoke() != null;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
