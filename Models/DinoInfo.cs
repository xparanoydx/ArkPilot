using ArkPilot.Data;

namespace ArkPilot.Models
{
    public class DinoInfo
    {
        public string Name { get; set; } = "";

        public string Species { get; set; } = "";

        public int Level { get; set; }

        public string Gender { get; set; } = "";

        public string TribeName { get; set; } = "";

        public string OwnerName { get; set; } = "";

        public bool IsCryopodded { get; set; }

        public string Location { get; set; } = "";
    }
}