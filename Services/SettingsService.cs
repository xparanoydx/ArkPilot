using ArkPilot.Config;

namespace ArkPilot.Services
{
    public class SettingsService
    {
        public ServerConfig Config { get; private set; }

        public SettingsService()
        {
            Config = ConfigManager.Load();
        }

        public ServerConfig Load()
        {
            Config = ConfigManager.Load();
            return Config;
        }

        public void Save(ServerConfig config)
        {
            Config = config;
            ConfigManager.Save(config);

            LogService.Success("Configuration sauvegardée");
        }
    }
}