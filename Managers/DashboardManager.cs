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
                LastBackup = GetLastBackup(),
                ApiStatus = "--",
                Map = "--",
                Game = "--"
            };

            if (_nitradoService == null)
                return info;

            try
            {
                var nitradoInfo =
                    await _nitradoService.GetServerInfoAsync();

                if (nitradoInfo == null)
                    return info;

                info.ApiStatus =
                    NormalizeStatus(nitradoInfo.Status);

                info.Map =
                    NormalizeMap(nitradoInfo.Map);

                info.Game =
                    NormalizeGame(nitradoInfo.Version);

                info.SlotsUsed =
                    _monitor.PlayerCount;

                info.SlotsMax =
                    nitradoInfo.SlotsMax;
            }
            catch (Exception ex)
            {
                LogService.Warning(
                    $"Dashboard API Nitrado indisponible : {ex.Message}");

                info.ApiStatus = "Indisponible";
                info.Map = "--";
                info.Game = "--";
                info.SlotsUsed = 0;
                info.SlotsMax = 0;
            }

            return info;
        }

        private string GetUptime()
        {
            if (_monitor.ConnectedSince == default)
                return "--";

            TimeSpan uptime =
                DateTime.Now - _monitor.ConnectedSince;

            return $"{uptime:hh\\:mm\\:ss}";
        }

        private string GetLastBackup()
        {
            if (_backupService == null ||
                _backupService.LastBackupTime == default)
            {
                return "--";
            }

            return _backupService
                .LastBackupTime
                .ToString("HH:mm:ss");
        }

        private static string NormalizeStatus(
            string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "--";

            return status.ToLowerInvariant() switch
            {
                "started" => "Started",
                "stopped" => "Stopped",
                "restarting" => "Restarting",
                _ => status
            };
        }

        private static string NormalizeMap(
            string? map)
        {
            if (string.IsNullOrWhiteSpace(map))
                return "--";

            return map.Replace(
                "_WP",
                "",
                StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeGame(
            string? game)
        {
            if (string.IsNullOrWhiteSpace(game))
                return "--";

            return game.ToLowerInvariant() switch
            {
                "arksa" => "ARK Survival Ascended",
                _ => game
            };
        }
    }
}