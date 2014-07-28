using System;
using System.Windows.Input;

namespace Ficksworkshop.TimeTrackerAPI.Commands
{
    /// <summary>
    /// A standard command to punch into to or punch out of a project. If not punched in, this acts as a punch in,
    /// otherwise if punched in, this acts as a punch out.
    /// </summary>
    public class PunchInOutCommand : ICommand
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
        /// <param name="project">a function that returns the project that should be punched in/out
        /// This function may return null to indicate there is no project available to delete.</param>
        public PunchInOutCommand(Func<IProjectTimesData> commandContext, Func<IProject> project)
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
                IProjectTime activeTimeRow = dataSet.PunchedInTime();
                if (activeTimeRow != null)
                {
                    // No open time, so do a punch in
                    activeTimeRow.End = DateTime.Now;
                }
                else
                {
                    // Have an open time, so do a punch out
                    dataSet.CreateTime(project, DateTime.Now, null);
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>We can execute if there is a context, a project, and the project isn't closed.</remarks>
        public bool CanExecute(object parameter)
        {
            IProject project = _projectFunc.Invoke();
            return _commandContextFunc.Invoke() != null && project != null && project.Status != ProjectStatus.Closed;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
