using System.Collections.Generic;

namespace ArkPilot.Models
{
    public class ArkSaveDataResult
    {
        public List<TribeInfo> Tribes { get; set; }
            = new();

        public bool Success { get; set; }

        public string Error { get; set; }
            = "";
    }
}