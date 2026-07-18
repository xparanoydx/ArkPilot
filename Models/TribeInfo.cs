using System.Collections.Generic;

namespace ArkPilot.Models
{
    public class TribeInfo
    {
        public string Name { get; set; } = "";

        public string Id { get; set; } = "";

        public int MemberCount { get; set; }

        public List<string> Members { get; set; }
            = new();

        public List<TribeMemberInfo> MemberDetails { get; set; }
            = new();
    }
}