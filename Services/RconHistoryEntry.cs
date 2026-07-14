using System;

namespace ArkPilot.Services
{
    public class RconHistoryEntry
    {
        public DateTime Timestamp { get; set; }

        public string Command { get; set; } = "";

        public string Response { get; set; } = "";
    }
}