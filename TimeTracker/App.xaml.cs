using System.Windows;

using Hardcodet.Wpf.TaskbarNotification;

namespace Ficksworkshop.TimeTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _notificationIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
           base.OnStartup(e);

           _notificationIcon = (TaskbarIcon)FindResource("NotificationIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notificationIcon.Dispose();

            base.OnExit(e);
        }

    }
}
