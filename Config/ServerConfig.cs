using ArkPilot.Helpers;

namespace ArkPilot.Config
{
    public class ServerConfig
    {
        // =========================
        // SERVEUR
        // =========================

        public string ServerName { get; set; } = "Mon serveur ARK";

        public string ServerIp { get; set; } = "";

        public int RconPort { get; set; } = AppConstants.DefaultRconPort;

        public string RconPassword { get; set; } = "";

        // =========================
        // API NITRADO
        // =========================

        public string NitradoApiKey { get; set; } = "";

        public string NitradoServiceId { get; set; } = "";

        // =========================
        // FTP
        // =========================

        public string FtpHost { get; set; } = "";

        public int FtpPort { get; set; } = 21;

        public string FtpUser { get; set; } = "";

        public string FtpPassword { get; set; } = "";

        // =========================
        // APPLICATION
        // =========================

        public bool AutoConnect { get; set; } = true;

        public int RefreshInterval { get; set; } = 5;
    }
}