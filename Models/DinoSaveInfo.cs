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