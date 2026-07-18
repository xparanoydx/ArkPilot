using System.Collections.Generic;

namespace ArkPilot.Models
{
    public class DinoSaveDataResult
    {
        public List<DinoSaveInfo> Dinos { get; set; } = new();

        public string ErrorMessage { get; set; } = "";

        public bool Success =>
            string.IsNullOrWhiteSpace(ErrorMessage);
    }
}