using System;
using System.Collections.Concurrent;
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

        private Task? _workerTask;

        private bool _running;


        // =========================
        // EVENTS
        // =========================

        public event Action<string>? OnLog;

        public event Action<string, string>? OnResponse;



        public bool IsConnected =>
            _client.IsConnected;



        public RconEngine(RconClient client)
        {
            _client = client;

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

        public async Task<string> Query(
            string command)
        {
            var tcs =
                new TaskCompletionSource<string>();


            void Handler(
                string cmd,
                string result)
            {
                if (cmd != command)
                    return;


                tcs.TrySetResult(result);

                OnResponse -= Handler;
            }


            OnResponse += Handler;


            Send(command);



            var timeout =
                Task.Delay(5000);


            var completed =
                await Task.WhenAny(
                    tcs.Task,
                    timeout);



            if (completed == timeout)
            {
                OnResponse -= Handler;

                return "TIMEOUT";
            }


            return await tcs.Task;
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


                    // Ne pas afficher les commandes automatiques
                    // du ServerMonitor

                    if (command != "ListPlayers")
                    {
                        LogService.Info($"RCON -> {command}");
                        OnLog?.Invoke($"RCON -> {command}");
                    }
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