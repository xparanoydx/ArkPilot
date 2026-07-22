using ArkPilot.Data;
using ArkPilot.Helpers;
using ArkPilot.Services;
using System.IO;

namespace ArkPilot.Models
{
    public class DinoSaveInfo
    {
        // =========================
        // Identité
        // =========================

        public string Species { get; set; } = "";

        public string Name { get; set; } = "";

        public string ClassName { get; set; } = "";

        public string IconPath =>
            CreatureIconService.GetIconPathFromClassName(ClassName);

        public string? BadgePath
        {
            get
            {
                var creature =
                    CreatureDatabase.GetByClassName(ClassName);

                if (creature == null)
                    return null;

                return BadgeIconService.GetBadgePath(
                    creature.IsTek,
                    creature.IsAberrant,
                    creature.IsXCreature,
                    creature.IsRCreature,
                    creature.IsBoss,
                    creature.IsFantasy);
            }
        }

        public string CategoryDisplay
        {
            get
            {
                var creature =
                    CreatureDatabase.GetByClassName(ClassName);

                if (creature == null)
                    return "Inconnu";

                return creature.Category switch
                {
                    CreatureCategory.Carnivore => "🥩 Carnivore",
                    CreatureCategory.Herbivore => "🌿 Herbivore",
                    CreatureCategory.Flyer => "🪽 Volant",
                    CreatureCategory.Aquatic => "🌊 Aquatique",
                    CreatureCategory.Structure => "🏗 Structure",
                    _ => creature.Category.ToString()
                };
            }
        }

        public long DinoId { get; set; }

        public string Gender { get; set; } = "";


        // =========================
        // Niveaux
        // =========================

        public int BaseLevel { get; set; }

        public int Level { get; set; }

        // =========================
        // Localisation
        // =========================

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string Gps =>
            Latitude.HasValue && Longitude.HasValue
                ? $"{Latitude.Value:0.0} / {Longitude.Value:0.0}"
                : "";

        // =========================
        // Appartenance
        // =========================

        public int TribeId { get; set; }

        public string TribeName { get; set; } = "";

        public string OwnerName { get; set; } = "";

        // =========================
        // Élevage
        // =========================

        public string FatherName { get; set; } = "";

        public string MotherName { get; set; } = "";

        public int MaleMutations { get; set; }

        public int FemaleMutations { get; set; }

        // =========================
        // Cryo
        // =========================

        public bool IsCryopodded { get; set; }

        public string CryoDisplay =>
            IsCryopodded ? "❄" : "";
    }
}