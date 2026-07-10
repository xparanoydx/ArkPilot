namespace ArkPilot.Models
{
    public class NitradoServerInfo
    {
        public string Status { get; set; } = "Inconnu";

        public string Map { get; set; } = "--";

        public string Version { get; set; } = "--";

        public int SlotsUsed { get; set; }

        public int SlotsMax { get; set; }

        public string RawJson { get; set; } = "";
    }
}