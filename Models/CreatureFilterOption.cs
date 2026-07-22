using ArkPilot.Helpers;

namespace ArkPilot.Models
{
    public sealed class CreatureFilterOption
    {
        public CreatureFilter Filter { get; init; }

        public string DisplayName { get; init; } = "";

        public override string ToString()
        {
            return DisplayName;
        }
    }
}