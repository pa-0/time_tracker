using System;
using System.Windows.Input;

namespace Ficksworkshop.TimeTrackerAPI.Commands
{
    /// <summary>
    /// A standard command that can creates a new project.
    /// 
    /// This command required bound command pameter that specified the <see cref="IProjectTimesData"/> to add to.
    /// </summary>
    public class CreateProjectCommand : ICommand
    {
        #region Fields

        private readonly Func<string> _newUniqueIdFunc;

        private readonly Func<string> _newNameFunc;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProjectCommand"/> class.
        /// </summary>
        /// that should be used to execute command. This function may return null.</param>
        /// <param name="newUniqueId">A function that returns the unique ID to assign the project. May be null.</param>
        /// <param name="newName">A function that returns the name to assign the project. May be null.</param>
        public CreateProjectCommand(Func<string> newUniqueId, Func<string> newName)
        {
            _newUniqueIdFunc = newUniqueId;
            _newNameFunc = newName;
        }

        #endregion

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            var dataSet = parameter as IProjectTimesData;
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
            var dataSet = parameter as IProjectTimesData;
            return dataSet != null;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}