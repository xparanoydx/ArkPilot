using System;
using System.Threading.Tasks;
using ArkPilot.Models;
using ArkPilot.Services;

namespace ArkPilot.Managers
{
    public class DashboardManager
    {
        private readonly ServerMonitor _monitor;
        private readonly BackupService? _backupService;
        private readonly NitradoService? _nitradoService;

        public DashboardManager(
            ServerMonitor monitor,
            BackupService? backupService = null,
            NitradoService? nitradoService = null)
        {
            _monitor = monitor;
            _backupService = backupService;
            _nitradoService = nitradoService;
        }

        public async Task<DashboardInfo> GetDashboardInfoAsync()
        {
            var info = new DashboardInfo
            {
                Online = _monitor.Online,
                Ping = _monitor.Ping,
                Players = _monitor.PlayerCount,
                Uptime = GetUptime(),
                LastBackup = GetLastBackup()
            };

            if (_nitradoService != null)
            {
                var nitradoInfo =
                    await _nitradoService.GetServerInfoAsync();

                info.ApiStatus = NormalizeStatus(nitradoInfo.Status);
                info.Map = NormalizeMap(nitradoInfo.Map);
                info.Game = NormalizeGame(nitradoInfo.Version);
                info.SlotsUsed = nitradoInfo.SlotsUsed;
                info.SlotsMax = nitradoInfo.SlotsMax;
            }

            return info;
        }

        private string GetUptime()
        {
            if (_monitor.ConnectedSince == default)
                return "--";

            TimeSpan uptime = DateTime.Now - _monitor.ConnectedSince;

            return $"{uptime:hh\\:mm\\:ss}";
        }

        private string GetLastBackup()
        {
            if (_backupService == null ||
                _backupService.LastBackupTime == default)
            {
                return "--";
            }

            return _backupService.LastBackupTime.ToString("HH:mm:ss");
        }

        private static string NormalizeStatus(string status)
        {
            return status.ToLower() switch
            {
                "started" => "Started",
                "stopped" => "Stopped",
                "restarting" => "Restarting",
                _ => status
            };
        }

        private static string NormalizeMap(string map)
        {
            return map.Replace("_WP", "");
        }

        private static string NormalizeGame(string game)
        {
            return game.ToLower() switch
            {
                "arksa" => "ARK Survival Ascended",
                _ => game
            };
        }
    }
}