using ArkPilot.Data;
using ArkPilot.Models;
using System.Collections.Generic;
using System;
using System.Linq;

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

            CreatureInfo? creature =
                GetCreatureInfo(species);


            return creature?.DisplayName ?? species;
        }



        public static CreatureInfo? GetCreatureInfo(
            string species)
        {
            return CreatureDatabase.Find(species);
        }


        public static IReadOnlyCollection<CreatureInfo>
            GetAll()
        {
            return CreatureDatabase.Creatures.Values;
        }

        public static IEnumerable<CreatureInfo> GetByCategory(
    CreatureCategory category)
        {
            return CreatureDatabase.GetByCategory(category);
        }

        public static IEnumerable<CreatureInfo> GetFlyers()
        {
            return CreatureDatabase.GetFlyers();
        }

        public static IEnumerable<CreatureInfo> GetAquatics()
        {
            return CreatureDatabase.GetAquatics();
        }

        public static IEnumerable<CreatureInfo> GetTekCreatures()
        {
            return CreatureDatabase.GetTekCreatures();
        }

        public static IEnumerable<CreatureInfo> GetBosses()
        {
            return CreatureDatabase.GetBosses();
        }

        public static IEnumerable<CreatureInfo> GetFantasyCreatures()
        {
            return CreatureDatabase.GetFantasyCreatures();
        }

        public static IEnumerable<CreatureInfo> GetByFilter(
    CreatureFilter filter)
        {
            return filter switch
            {
                CreatureFilter.Herbivores =>
                    GetByCategory(CreatureCategory.Herbivore),

                CreatureFilter.Carnivores =>
                    GetByCategory(CreatureCategory.Carnivore),

                CreatureFilter.Omnivores =>
                    GetByCategory(CreatureCategory.Omnivore),

                CreatureFilter.Flyers =>
                    GetFlyers(),

                CreatureFilter.Aquatics =>
                    GetAquatics(),

                CreatureFilter.Insects =>
                    GetByCategory(CreatureCategory.Insect),

                CreatureFilter.Fantasy =>
                    GetFantasyCreatures(),

                CreatureFilter.Tek =>
                    GetTekCreatures(),

                CreatureFilter.Bosses =>
                    GetBosses(),

                CreatureFilter.Structure =>
                    GetByCategory(CreatureCategory.Structure),

                _ =>
                    GetAll()
            };
        }

        public static IReadOnlyList<CreatureFilterOption>
    GetFilterOptions()
        {
            return Enum
                .GetValues<CreatureFilter>()
                .Select(filter => new CreatureFilterOption
                {
                    Filter = filter,
                    DisplayName = filter.GetDisplayName()
                })
                .ToList();
        }

        public static int Count =>
    CreatureDatabase.Count;
    }
}