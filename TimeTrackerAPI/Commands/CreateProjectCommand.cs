using System;
using System.Windows.Input;
using Ficksworkshop.TimeTracker.Model;

namespace Ficksworkshop.TimeTracker.Commands
{
    /// <summary>
    /// A standard command that can creates a new project.
    /// 
    /// This command requires a function to indicate which project data set to modify. Usually, can use a function
    /// defined in <see cref="TrackerInstance"/> that returns a singleton instance.
    /// </summary>
    public class CreateProjectCommand : ICommand
    {
        #region Fields

        private readonly Func<IProjectTimesData> _projectTimesFunc;

        private readonly Func<string> _newUniqueIdFunc;

        private readonly Func<string> _newNameFunc;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProjectCommand"/> class.
        /// </summary>
        /// that should be used to execute command. This function may return null.</param>
        /// <param name="projectTimes">A function that returns the project times to modify.</param>
        /// <param name="newUniqueId">A function that returns the unique ID to assign the project. May be null.</param>
        /// <param name="newName">A function that returns the name to assign the project. May be null.</param>
        public CreateProjectCommand(Func<IProjectTimesData> projectTimes, Func<string> newUniqueId, Func<string> newName)
        {
            _newUniqueIdFunc = newUniqueId;
            _newNameFunc = newName;
            _projectTimesFunc = projectTimes;
        }

        #endregion

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            var dataSet = _projectTimesFunc.Invoke();
            if (dataSet != null)
            {
                string newUniqueId = (_newUniqueIdFunc != null) ? _newUniqueIdFunc.Invoke() : "";
                string newName = (_newNameFunc != null) ? _newNameFunc.Invoke() : "";
                dataSet.CreateProject(newUniqueId, newName);
            }
        }

        /// <inheritdoc />
        /// <remarks>We can execute if there is a context and a project.</remarks>
        public bool CanExecute(object parameter)
        {
            return _projectTimesFunc.Invoke() != null;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}