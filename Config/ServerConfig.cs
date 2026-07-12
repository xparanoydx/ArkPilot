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
        // AUTOMATION
        // =========================

        public bool AutoSaveEnabled { get; set; } = true;

        public int AutoSaveIntervalMinutes { get; set; } = 30;

        public bool AutoRestartEnabled { get; set; } = true;

        public int AutoRestartHour { get; set; } = 6;

        public int AutoRestartMinute { get; set; } = 0;


        // =========================
        // WEEKEND EVENT
        // =========================

        public int WeekendStartDay { get; set; } = 5;

        public int WeekendStartHour { get; set; } = 18;

        public int WeekendStartMinute { get; set; } = 0;

        public int WeekendEndDay { get; set; } = 0;

        public int WeekendEndHour { get; set; } = 23;

        public int WeekendEndMinute { get; set; } = 59;

        public bool WeekendEventEnabled { get; set; } = false;

        public bool WeekendTamingEnabled { get; set; } = true;
        public double WeekendTamingMultiplier { get; set; } = 2.0;

        public bool WeekendHarvestEnabled { get; set; } = true;
        public double WeekendHarvestMultiplier { get; set; } = 3.0;

        public bool WeekendBabyMatureEnabled { get; set; } = true;
        public double WeekendBabyMatureMultiplier { get; set; } = 2.0;

        public bool WeekendXpEnabled { get; set; } = true;
        public double WeekendXpMultiplier { get; set; } = 2.0;

        public bool WeekendWildDinoFoodDrainEnabled { get; set; } = true;
        public double WeekendWildDinoFoodDrainMultiplier { get; set; } = 1.0;

        public bool WeekendBabyCuddleIntervalEnabled { get; set; } = true;
        public double WeekendBabyCuddleIntervalMultiplier { get; set; } = 1.0;

        // =========================
        // APPLICATION
        // =========================

        public bool AutoConnect { get; set; } = true;

        public int RefreshInterval { get; set; } = 5;
    }
}