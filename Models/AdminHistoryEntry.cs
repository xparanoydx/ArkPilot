using System;

namespace ArkPilot.Models
{
    public class AdminHistoryEntry
    {
        public DateTime Timestamp { get; set; }

        public string Action { get; set; } = "";

        public string Target { get; set; } = "";

        public string Details { get; set; } = "";
    }
}