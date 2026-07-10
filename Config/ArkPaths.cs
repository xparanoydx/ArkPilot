namespace ArkPilot.Config
{
    public static class ArkPaths
    {
        // Dossier de sauvegarde du monde
        public const string SaveFolder =
            "/arksa/ShooterGame/Saved/SavedArks/Ragnarok_WP/";

        // Monde
        public const string World =
            SaveFolder + "Ragnarok_WP.ark";

        // Configurations
        public const string ConfigFolder =
            "/arksa/ShooterGame/Saved/Config/WindowsServer/";

        public const string GameIni =
            ConfigFolder + "Game.ini";

        public const string GameUserSettingsIni =
            ConfigFolder + "GameUserSettings.ini";
    }
}