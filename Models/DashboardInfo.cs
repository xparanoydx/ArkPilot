namespace ArkPilot.Models
{
    public class DashboardInfo
    {
        public bool Online { get; set; }

        public int Ping { get; set; }

        public int Players { get; set; }

        public string Uptime { get; set; } = "--";

        public string LastBackup { get; set; } = "--";

        public string ApiStatus { get; set; } = "--";

        public string Map { get; set; } = "--";

        public string Game { get; set; } = "--";

        public int SlotsUsed { get; set; }

        public int SlotsMax { get; set; }
    }
}