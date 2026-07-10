using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArkPilot.Helpers;

namespace ArkPilot.Services
{
    public class ServerMonitor
    {
        private readonly RconEngine _rcon;

        private readonly string _ip;
        private readonly int _port;
        private readonly string _password;

        private readonly CancellationTokenSource _cts = new();

        private bool _started;


        public bool Online { get; private set; }

        public int Ping { get; private set; } = -1;

        public int PlayerCount { get; private set; }

        public DateTime LastUpdate { get; private set; }

        public DateTime ConnectedSince { get; private set; }


        public List<PlayerInfo> Players { get; } = new();


        public event Action? Updated;



        public ServerMonitor(
            RconEngine rcon,
            string ip,
            int port,
            string password)
        {
            _rcon = rcon;

            _ip = ip;
            _port = port;
            _password = password;
        }



        // =========================
        // START
        // =========================

        public void Start()
        {
            if (_started)
                return;

            _started = true;

            _ = Task.Run(
                MonitorLoop);
        }



        // =========================
        // LOOP
        // =========================

        private async Task MonitorLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                await Refresh();


                await Task.Delay(
                    AppConstants.MonitorRefreshMs);
            }
        }



        // =========================
        // REFRESH
        // =========================

        private async Task Refresh()
        {
            try
            {
                if (!_rcon.IsConnected)
                {
                    await TryReconnect();

                    if (!_rcon.IsConnected)
                    {
                        SetOffline();
                        return;
                    }
                }



                var start =
                    DateTime.Now;


                string result =
                    await _rcon.Query(
                        "ListPlayers");



                Ping =
                    (int)(DateTime.Now - start)
                    .TotalMilliseconds;



                if (result == "TIMEOUT" ||
                    result == "RCON_OFFLINE" ||
                    result.StartsWith("RCON ERROR"))
                {
                    SetOffline();

                    return;
                }



                Players.Clear();



                foreach (var line in result.Split(
                    '\n',
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!line.Contains(","))
                        continue;


                    var parts =
                        line.Split(',', 2);


                    Players.Add(
                        new PlayerInfo
                        {
                            Name = parts[0].Trim(),
                            Id = parts[1].Trim()
                        });
                }



                PlayerCount =
                    Players.Count;


                Online = true;

                if (ConnectedSince == default)
                {
                    ConnectedSince = DateTime.Now;
                }


                LastUpdate =
                    DateTime.Now;



                Updated?.Invoke();
            }
            catch (Exception ex)
            {
                LogService.Error($"Erreur ServerMonitor : {ex.Message}");
                SetOffline();
            }
        }



        // =========================
        // RECONNECTION
        // =========================

        private async Task TryReconnect()
        {
            LogService.Warning("RCON déconnecté, tentative de reconnexion...");

            bool connected =
                await _rcon.Connect(
                    _ip,
                    _port,
                    _password);

            if (connected)
            {
                LogService.Success("Reconnexion RCON réussie");
            }
            else
            {
                LogService.Error("Reconnexion RCON échouée");
            }
        }


        // =========================
        // OFFLINE
        // =========================

        private void SetOffline()
        {
            Online = false;

            Ping = -1;

            PlayerCount = 0;

            Players.Clear();

            Updated?.Invoke();

            ConnectedSince = default;
        }



        // =========================
        // STOP
        // =========================

        public void Stop()
        {
            _cts.Cancel();
        }
    }
}