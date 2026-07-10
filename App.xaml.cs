using System.Windows;
using ArkPilot.Services;

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