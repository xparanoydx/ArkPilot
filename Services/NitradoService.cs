using System;
using System.Text.Json;
using System.Threading.Tasks;
using ArkPilot.Config;
using ArkPilot.Models;

namespace ArkPilot.Services
{
    public class NitradoService
    {
        private readonly ServerConfig _config;

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(_config.NitradoApiKey) &&
            !string.IsNullOrWhiteSpace(_config.NitradoServiceId);

        public NitradoService(ServerConfig config)
        {
            _config = config;
        }

        private NitradoApi CreateApi()
        {
            return new NitradoApi(
                _config.NitradoApiKey,
                _config.NitradoServiceId);
        }

        public async Task<string> GetStatusAsync()
        {
            if (!IsConfigured)
            {
                LogService.Warning("API Nitrado non configurée");
                return "NITRADO_NOT_CONFIGURED";
            }

            try
            {
                LogService.Info("Lecture de l'état Nitrado");

                var api = CreateApi();

                return await api.GetStatus();
            }
            catch (Exception ex)
            {
                LogService.Error($"Erreur API Nitrado : {ex.Message}");
                return "NITRADO_ERROR";
            }
        }

        public async Task<NitradoServerInfo> GetServerInfoAsync()
        {
            var info = new NitradoServerInfo();

            string json = await GetStatusAsync();

            info.RawJson = json;

            if (json == "NITRADO_NOT_CONFIGURED")
            {
                info.Status = "Non configurée";
                return info;
            }

            if (json == "NITRADO_ERROR")
            {
                info.Status = "Erreur API";
                return info;
            }

            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);

                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("data", out JsonElement data) &&
                    data.TryGetProperty("gameserver", out JsonElement gameserver))
                {
                    if (gameserver.TryGetProperty("status", out JsonElement status))
                    {
                        info.Status = status.GetString() ?? "Inconnu";
                    }

                    if (gameserver.TryGetProperty("game", out JsonElement game))
                    {
                        info.Version = game.GetString() ?? "--";
                    }

                    if (gameserver.TryGetProperty("slots", out JsonElement slots))
                    {
                        info.SlotsMax = slots.GetInt32();
                    }

                    if (gameserver.TryGetProperty("query", out JsonElement query))
                    {
                        if (query.TryGetProperty("map", out JsonElement map))
                        {
                            info.Map = map.GetString() ?? "--";
                        }

                        if (query.TryGetProperty("player_current", out JsonElement current))
                        {
                            info.SlotsUsed = current.GetInt32();
                        }

                        if (query.TryGetProperty("player_max", out JsonElement max))
                        {
                            info.SlotsMax = max.GetInt32();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                info.Status = "Erreur parsing";
                LogService.Error($"Parsing Nitrado impossible : {ex.Message}");
            }

            return info;
        }

        public async Task<string> StartAsync()
        {
            if (!IsConfigured)
                return "NITRADO_NOT_CONFIGURED";

            try
            {
                LogService.Info("Démarrage du serveur via Nitrado");

                var api = CreateApi();

                return await api.StartServer();
            }
            catch (Exception ex)
            {
                LogService.Error($"Erreur démarrage Nitrado : {ex.Message}");
                return "NITRADO_ERROR";
            }
        }

        public async Task<string> StopAsync()
        {
            if (!IsConfigured)
                return "NITRADO_NOT_CONFIGURED";

            try
            {
                LogService.Warning("Arrêt du serveur via Nitrado");

                var api = CreateApi();

                return await api.StopServer();
            }
            catch (Exception ex)
            {
                LogService.Error($"Erreur arrêt Nitrado : {ex.Message}");
                return "NITRADO_ERROR";
            }
        }

        public async Task<string> RestartAsync()
        {
            if (!IsConfigured)
                return "NITRADO_NOT_CONFIGURED";

            try
            {
                LogService.Warning("Redémarrage du serveur via Nitrado");

                var api = CreateApi();

                return await api.RestartServer();
            }
            catch (Exception ex)
            {
                LogService.Error($"Erreur redémarrage Nitrado : {ex.Message}");
                return "NITRADO_ERROR";
            }
        }
    }
}