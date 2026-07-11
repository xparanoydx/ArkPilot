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

                    NitradoServiceId =
                        config.NitradoServiceId,

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