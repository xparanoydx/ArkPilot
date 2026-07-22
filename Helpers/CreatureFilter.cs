namespace ArkPilot.Helpers
{
    public enum CreatureFilter
    {
        All,
        Herbivores,
        Carnivores,
        Omnivores,
        Flyers,
        Aquatics,
        Insects,
        Fantasy,
        Tek,
        Structure,
        Bosses
    }

    public static class CreatureFilterExtensions
    {
        public static string GetDisplayName(
            this CreatureFilter filter)
        {
            return filter switch
            {
                CreatureFilter.All =>
                    "Toutes les créatures",

                CreatureFilter.Herbivores =>
                    "Herbivores",

                CreatureFilter.Carnivores =>
                    "Carnivores",

                CreatureFilter.Omnivores =>
                    "Omnivores",

                CreatureFilter.Flyers =>
                    "Volants",

                CreatureFilter.Aquatics =>
                    "Aquatiques",

                CreatureFilter.Insects =>
                    "Insectes",

                CreatureFilter.Fantasy =>
                    "Fantastiques",

                CreatureFilter.Tek =>
                    "Tek",

                CreatureFilter.Bosses =>
                    "Boss",

                CreatureFilter.Structure =>
                    "🛶 Structures",

                _ =>
                    filter.ToString()
            };
        }
    }
}