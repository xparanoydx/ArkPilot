using System.Diagnostics;

namespace ArkPilot.Services
{
    public static class ProcessCleaner
    {
        public static void KillGhostProcesses()
        {
            var processes = Process.GetProcessesByName("ArkPilot");

            foreach (var p in processes)
            {
                try
                {
                    if (!p.HasExited)
                    {
                        p.Kill(true);
                    }
                }
                catch
                {
                    // ignore (process déjà mort ou protégé)
                }
            }
        }
    }
}