using System;

namespace ArkPilot.Models
{
    public class PlayerNote
    {
        public string PlayerId { get; set; } = "";

        public string PlayerName { get; set; } = "";

        public string Note { get; set; } = "";

        public DateTime UpdatedAt { get; set; }
    }
}