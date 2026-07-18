using ArkPilot.Data;
using ArkPilot.Models;
using System.Collections.Generic;

namespace ArkPilot.Helpers
{
    public static class ArkSpeciesHelper
    {
        public static string GetDisplayName(
            string species)
        {
            if (string.IsNullOrWhiteSpace(species))
            {
                return "Inconnu";
            }

            return CreatureDatabase.Creatures.TryGetValue(
                species,
                out CreatureInfo? creature)
                    ? creature.DisplayName
                    : species;
        }

        public static CreatureInfo? GetCreatureInfo(
            string species)
        {
            if (string.IsNullOrWhiteSpace(species))
            {
                return null;
            }

            return CreatureDatabase.Creatures.TryGetValue(
                species,
                out CreatureInfo? creature)
                    ? creature
                    : null;
        }

        public static IReadOnlyCollection<CreatureInfo>
            GetAll()
        {
            return CreatureDatabase.Creatures.Values;
        }

        public static int Count =>
    CreatureDatabase.Count;
    }
}