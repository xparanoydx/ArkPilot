using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ArkPilot.Services
{
    public class RconEngine : IDisposable
    {
        private readonly RconClient _client;

        private readonly ConcurrentQueue<string> _queue = new();

        private readonly SemaphoreSlim _commandLock = new(1, 1);

        private readonly CancellationTokenSource _cts = new();

        private readonly RconHistoryService _historyService = new();

        private Task? _workerTask;

        private bool _running;


        // =========================
        // EVENTS
        // =========================

        public event Action<string>? OnLog;

        public event Action<string, string>? OnResponse;

        public ObservableCollection<RconHistoryEntry> History { get; }
            = new();



        public bool IsConnected =>
            _client.IsConnected;



        public RconEngine(RconClient client)
        {
            _client = client;

            foreach (var entry in _historyService.Load())
            {
                History.Add(entry);
            }


            while (History.Count > 300)
            {
                History.RemoveAt(0);
            }

            _client.OnLog += msg =>
            {
                OnLog?.Invoke(msg);
            };
        }



        // =========================
        // CONNECT
        // =========================

        public async Task<bool> Connect(
            string ip,
            int port,
            string password)
        {
            bool result =
                await _client.Connect(
                    ip,
                    (ushort)port,
                    password);


            if (result && !_running)
            {
                _running = true;

                _workerTask =
                    Task.Run(
                        Worker);
            }


            return result;
        }



        // =========================
        // SEND ASYNC QUEUE
        // =========================

        public void Send(string command)
        {
            if (!IsConnected)
            {
                return;
            }


            _queue.Enqueue(command);
        }



        // =========================
        // DIRECT COMMAND
        // =========================

        public async Task<string> SendDirect(
            string command)
        {
            if (!IsConnected)
            {
                return "RCON_OFFLINE";
            }


            await _commandLock.WaitAsync();


            try
            {
                string result =
                    await _client.Send(command);


                OnResponse?.Invoke(
                    command,
                    result);

                History.Add(
                    new RconHistoryEntry
                {
                    Timestamp = DateTime.Now,
                    Command = command,
                    Response = result
                });

                while (History.Count > 300)
                {
                    History.RemoveAt(0);
                }

                _historyService.Save(
                    History);

                return result;
            }
            finally
            {
                _commandLock.Release();
            }
        }



        // =========================
        // QUERY WITH RESPONSE
        // =========================

        // =========================
        // SILENT INTERNAL QUERY
        // =========================

        public async Task<string> Query(
            string command)
        {
            if (!IsConnected)
            {
                return "RCON_OFFLINE";
            }

            await _commandLock.WaitAsync();

            try
            {
                string result =
                    await _client.Send(command);

                return result;
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"RCON query erreur : {ex.Message}");

                return "RCON_ERROR";
            }
            finally
            {
                _commandLock.Release();
            }
        }


        // =========================
        // WORKER
        // =========================

        private async Task Worker()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (!_queue.TryDequeue(out var command))
                {
                    await Task.Delay(100);
                    continue;
                }


                if (!IsConnected)
                {
                    continue;
                }


                await _commandLock.WaitAsync();

                try
                {
                    string result =
                        await _client.Send(command);


                    OnResponse?.Invoke(
                        command,
                        result);

                    History.Add(
                        new RconHistoryEntry
                    {
                        Timestamp = DateTime.Now,
                        Command = command,
                        Response = result
                    });

                    while (History.Count > 300)
                    {
                        History.RemoveAt(0);
                    }

                    _historyService.Save(
                        History);

                    // Ne pas afficher les commandes automatiques
                    // du ServerMonitor

                    LogService.Info(
                        $"RCON -> {command}");

                }
                catch (Exception ex)
                {
                    LogService.Error($"RCON erreur : {ex.Message}");
                    OnLog?.Invoke($"RCON erreur : {ex.Message}");
                }
                finally
                {
                    _commandLock.Release();
                }
            }
        }


        // =========================
        // CLEAR HISTORY
        // =========================

        public void ClearHistory()
        {
            History.Clear();

            _historyService.Save(
                History);

            LogService.Info(
                "Historique RCON effacé");
        }

        // =========================
        // STOP ENGINE
        // =========================

        public void Stop()
        {
            try
            {
                _cts.Cancel();

                _queue.Clear();

                _client.Disconnect();

                _running = false;

                LogService.Warning("RCON arrêté");
                OnLog?.Invoke("RCON arrêté");
            }
            catch
            {
            }
        }

        // =========================
        // DISPOSE
        // =========================

        public void Dispose()
        {
            _cts.Cancel();

            _client.Dispose();
        }
    }
}