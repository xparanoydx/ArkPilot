using ArkPilot.Services;
using ArkPilot.Views;
using System.Windows;

namespace ArkPilot
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ProcessCleaner.KillGhostProcesses();
            base.OnStartup(e);

        }
    }
}