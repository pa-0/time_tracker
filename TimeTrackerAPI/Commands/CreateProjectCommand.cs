using System;
using System.Windows.Input;

namespace Ficksworkshop.TimeTrackerAPI.Commands
{
    /// <summary>
    /// A standard command that can creates a new project.
    /// </summary>
    public class CreateProjectCommand : ICommand
    {
        #region Fields

        private readonly Func<IProjectTimesData> _commandContextFunc;

        private readonly Func<string> _newUniqueIdFunc;

        private readonly Func<string> _newNameFunc;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProjectCommand"/> class.
        /// </summary>
        /// <param name="commandContext">A function that returns an <see cref="IProjectTimesData"/>
        /// that should be used to execute command. This function may return null.</param>
        /// <param name="newUniqueId">A function that returns the unique ID to assign the project. May be null.</param>
        /// /// <param name="newName">A function that returns the name to assign the project. May be null.</param>
        public CreateProjectCommand(Func<IProjectTimesData> commandContext, Func<string> newUniqueId, Func<string> newName)
        {
            _commandContextFunc = commandContext;
            _newUniqueIdFunc = newUniqueId;
            _newNameFunc = newName;
        }

        #endregion

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            IProjectTimesData dataSet = _commandContextFunc.Invoke();
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
            return _commandContextFunc.Invoke() != null;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}