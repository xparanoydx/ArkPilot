namespace ArkPilot.Models
{
    public class PlayerSaveInfo
    {
        public string PlayerName { get; set; } = "";

        public string CharacterName { get; set; } = "";

        public string UniqueNetId { get; set; } = "";

        public string PlayerDataId { get; set; } = "";

        public string TribeName { get; set; } = "";

        public int Level { get; set; }
    }
}