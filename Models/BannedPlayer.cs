using System;

namespace ArkPilot.Models
{
    public class BannedPlayer
    {
        public string PlayerId { get; set; } = "";

        public string PlayerName { get; set; } = "";

        public DateTime BannedAt { get; set; }
    }
}