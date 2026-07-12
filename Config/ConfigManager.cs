using ArkPilot.Core;
using System;
using System.IO;
using System.Text.Json;

namespace ArkPilot.Config
{
    public static class ConfigManager
    {
        private static readonly string ConfigFolder =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ArkPilot");

        private static readonly string ConfigFile =
            Path.Combine(
                ConfigFolder,
                "serverconfig.json");

        public static ServerConfig Load()
        {
            Directory.CreateDirectory(
                ConfigFolder);

            if (!File.Exists(ConfigFile))
            {
                var cfg =
                    new ServerConfig();

                Save(cfg);

                return cfg;
            }

            string json =
                File.ReadAllText(ConfigFile);

            var config =
                JsonSerializer.Deserialize<ServerConfig>(json)
                ?? new ServerConfig();

            config.RconPassword =
                CryptoService.Decrypt(
                    config.RconPassword);

            config.NitradoApiKey =
                CryptoService.Decrypt(
                    config.NitradoApiKey);

            config.FtpPassword =
                CryptoService.Decrypt(
                    config.FtpPassword);

            return config;
        }

        public static void Save(
            ServerConfig config)
        {
            Directory.CreateDirectory(
                ConfigFolder);

            var protectedConfig =
                new ServerConfig
                {
                    ServerName =
                        config.ServerName,

                    ServerIp =
                        config.ServerIp,

                    RconPort =
                        config.RconPort,

                    RconPassword =
                        CryptoService.Encrypt(
                            config.RconPassword),

                    NitradoApiKey =
                        CryptoService.Encrypt(
                            config.NitradoApiKey),

                AutoSaveEnabled = config.AutoSaveEnabled,
                AutoSaveIntervalMinutes = config.AutoSaveIntervalMinutes,

                AutoRestartEnabled = config.AutoRestartEnabled,
                AutoRestartHour = config.AutoRestartHour,
                AutoRestartMinute = config.AutoRestartMinute,

                WeekendEventEnabled = config.WeekendEventEnabled,

                WeekendTamingMultiplier =
                 config.WeekendTamingMultiplier,

                WeekendWildDinoFoodDrainMultiplier =
                 config.WeekendWildDinoFoodDrainMultiplier,

                WeekendHarvestMultiplier =
                 config.WeekendHarvestMultiplier,

                WeekendBabyMatureMultiplier =
                 config.WeekendBabyMatureMultiplier,

                WeekendBabyCuddleIntervalMultiplier =
                 config.WeekendBabyCuddleIntervalMultiplier,

                WeekendXpMultiplier =
                 config.WeekendXpMultiplier,

                AutoConnect = config.AutoConnect,
                RefreshInterval = config.RefreshInterval,

                WeekendTamingEnabled =
                    config.WeekendTamingEnabled,

                WeekendHarvestEnabled =
                    config.WeekendHarvestEnabled,

                WeekendBabyMatureEnabled =
                    config.WeekendBabyMatureEnabled,

                WeekendXpEnabled =
                    config.WeekendXpEnabled,

                WeekendWildDinoFoodDrainEnabled =
                    config.WeekendWildDinoFoodDrainEnabled,

                WeekendBabyCuddleIntervalEnabled =
                    config.WeekendBabyCuddleIntervalEnabled,

                WeekendStartDay =
                    config.WeekendStartDay,

                WeekendStartHour =
                    config.WeekendStartHour,

                WeekendStartMinute =
                    config.WeekendStartMinute,

                WeekendEndDay =
                    config.WeekendEndDay,

                WeekendEndHour =
                    config.WeekendEndHour,

                WeekendEndMinute =
                    config.WeekendEndMinute,
            };

                    FtpHost =
                        config.FtpHost,

                    FtpPort =
                        config.FtpPort,

                    FtpUser =
                        config.FtpUser,

                    FtpPassword =
                        CryptoService.Encrypt(
                            config.FtpPassword),

                    AutoConnect =
                        config.AutoConnect,

                    RefreshInterval =
                        config.RefreshInterval
                };

            string json =
                JsonSerializer.Serialize(
                    protectedConfig,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(
                ConfigFile,
                json);
        }
    }
}