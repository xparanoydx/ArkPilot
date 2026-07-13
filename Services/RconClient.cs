using CoreRCON;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ArkPilot.Services
{
    public class RconClient : IDisposable
    {
        private RCON? _rcon;

        public bool IsConnected => _rcon != null;


        public event Action<string>? OnLog;


        // =========================
        // CONNEXION
        // =========================

        public async Task<bool> Connect(
            string ip,
            ushort port,
            string password)
        {
            try
            {
                Disconnect();


                var endpoint = new IPEndPoint(
                    IPAddress.Parse(ip),
                    port);


                _rcon = new RCON(
                    endpoint,
                    password);


                await _rcon.ConnectAsync();


                LogService.Success("RCON connecté");
                OnLog?.Invoke("✔ RCON connecté");

                return true;
            }
            catch (Exception ex)
            {
                Disconnect();

                LogService.Error($"Connexion RCON impossible : {ex.Message}");
                OnLog?.Invoke($"❌ Connexion RCON impossible : {ex.Message}");

                return false;
            }
        }



        // =========================
        // ENVOI COMMANDE
        // =========================

        public async Task<string> Send(
            string command)
        {
            if (_rcon == null)
            {
                return "RCON_OFFLINE";
            }


            try
            {
                var result =
                    await _rcon
                        .SendCommandAsync(command)
                        .WaitAsync(
                            TimeSpan.FromSeconds(10));


                return result ?? "";
            }
            catch (TimeoutException)
            {
                Disconnect();


                LogService.Warning(
                    $"Timeout RCON commande : {command}");

                OnLog?.Invoke(
                    $"⚠ Timeout RCON commande : {command}");


                return "TIMEOUT";
            }
            catch (Exception ex)
            {
                Disconnect();


                LogService.Warning(
                    $"RCON perdu : {ex.Message}");

                OnLog?.Invoke(
                    $"⚠ RCON perdu : {ex.Message}");


                return "RCON_OFFLINE";
            }
        }


        // =========================
        // DECONNEXION
        // =========================

        public void Disconnect()
        {
            try
            {
                _rcon?.Dispose();
            }
            catch
            {
            }


            _rcon = null;
        }



        public void Dispose()
        {
            Disconnect();
        }
    }
}