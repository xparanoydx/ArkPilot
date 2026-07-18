using System.Collections.Generic;

namespace ArkPilot.Models
{
    public class PlayerSaveDataResult
    {
        public List<PlayerSaveInfo> Players { get; set; }
            = new();

        public bool Success { get; set; }

        public string Error { get; set; }
            = "";
    }
}