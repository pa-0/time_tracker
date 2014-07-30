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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProjectCommand"/> class.
        /// </summary>
        /// <param name="commandContext">A function that returns an <see cref="IProjectTimesData"/>
        /// that should be used to execute command. This function may return null.</param>
        public CreateProjectCommand(Func<IProjectTimesData> commandContext)
        {
            _commandContextFunc = commandContext;
        }

        #endregion

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            IProjectTimesData dataSet = _commandContextFunc.Invoke();
            if (dataSet != null)
            {
                dataSet.CreateProject("", "");
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