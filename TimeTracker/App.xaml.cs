using System.Linq;
using System.Windows;
using Ficksworkshop.TimeTracker.Commands;
using Ficksworkshop.TimeTracker.Manager;
using Ficksworkshop.TimeTracker.Model;
using Ficksworkshop.TimeTracker.View;
using Hardcodet.Wpf.TaskbarNotification;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _notificationIcon;

        private SelectedProjectManager _selectedProjectManager;

        protected override void OnStartup(StartupEventArgs e)
        {
           base.OnStartup(e);

           RestoreLastState();

           _selectedProjectManager = new SelectedProjectManager();
           var autoPunchOut = new AutoPunchOutSelectedProjectEventHandler(_selectedProjectManager);

            // TODO hack this in there since I need to know the selection manager to be able to create the command
           Ficksworkshop.TimeTracker.Commands.Commands.PunchInOutCommand = new SelectedPunchInOutCommand(_selectedProjectManager);

           _notificationIcon = (TaskbarIcon)FindResource("NotificationIcon");
           _notificationIcon.DataContext = new NotificationIconViewModel(_selectedProjectManager);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notificationIcon.Dispose();

            base.OnExit(e);
        }

        private void RestoreLastState()
        {
            // Here should load settings from some default location. But not implemented storing of settings, so just set the values explicitly

            // Default load the database
            StringSetting setting = (StringSetting)TrackerInstance.Settings.Items.Where(i => i.Name == TrackerSettings.LastDataSet).First();

            // TODO this should be loading the settings selectively, not the database
            if (System.Diagnostics.Debugger.IsAttached)
            {
                setting.Value = @"D:\timetracker_debug.ttd";
            }
            else
            {
                setting.Value = @"D:\timetracker.ttd";
            }

            TrackerInstance.OpenDataSet(setting.Value);
        }
    }
}
