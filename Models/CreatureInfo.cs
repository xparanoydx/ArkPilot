using ArkPilot.Helpers;
using System;
using System.Collections.Generic;

namespace ArkPilot.Models
{
    public sealed class CreatureInfo
    {
        public string Species { get; init; } = "";

        public string ArkClass { get; set; } = "";

        public string DisplayName { get; init; } = "";

        public IReadOnlyList<string> Aliases { get; init; }
            = Array.Empty<string>();


        public CreatureCategory Category { get; init; } =
            CreatureCategory.Unknown;

        public bool IsTek { get; init; }

        public bool IsBoss { get; init; }

        public bool IsFlyer { get; init; }

        public bool IsAquatic { get; init; }

        public bool IsAberrant { get; init; }

        public bool IsXCreature { get; init; }

        public bool IsRCreature { get; init; }

        public bool IsFantasy { get; init; }
    }
}