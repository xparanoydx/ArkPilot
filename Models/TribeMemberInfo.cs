namespace ArkPilot.Models
{
    public class TribeMemberInfo
    {
        public string CharacterName { get; set; } = "";

        public string PlayerName { get; set; } = "";

        public string PlayerDataId { get; set; } = "";

        public int Level { get; set; }

        public bool IsOwner { get; set; }
    }
}