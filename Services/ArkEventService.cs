using ArkPilot.Config;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ArkPilot.Services
{
    public class ArkEventService
    {
        private readonly ServerConfig _config;

        private readonly FtpService _ftp;

        private const string ArkConfigPath =
            "/arksa/ShooterGame/Saved/Config/WindowsServer";

        private const string GameIniPath =
            ArkConfigPath + "/Game.ini";

        private const string GameUserSettingsPath =
            ArkConfigPath + "/GameUserSettings.ini";

        private static readonly string EventBackupPath =
    Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData),
        "ArkPilot",
        "Events",
        "Original");


        private static readonly string GameIniBackupPath =
            Path.Combine(
                EventBackupPath,
                "Game.ini");


        private static readonly string GameUserSettingsBackupPath =
            Path.Combine(
                EventBackupPath,
                "GameUserSettings.ini");


        public event Action<string>? OnLog;

        public bool HasOriginalConfigBackup =>
             File.Exists(GameIniBackupPath) &&
             File.Exists(GameUserSettingsBackupPath);


        public ArkEventService(
            ServerConfig config)
        {
            _config = config;


            _ftp = new FtpService(
                config.FtpHost,
                config.FtpPort,
                config.FtpUser,
                config.FtpPassword);
        }


        // =========================
        // TEST CONFIG FILES
        // =========================

        public async Task<bool> ApplyWeekendEventAsync()
        {
            OnLog?.Invoke(
                "🎉 Event : vérification des fichiers ARK");


            string? gameIni =
                await DownloadConfigFileAsync(
                    GameIniPath,
                    "Game.ini");


            string? gameUserSettings =
                await DownloadConfigFileAsync(
                    GameUserSettingsPath,
                    "GameUserSettings.ini");


            if (gameIni == null ||
                gameUserSettings == null)
            {
                OnLog?.Invoke(
                    "❌ Event : fichiers de configuration indisponibles");

                return false;
            }

            BackupOriginalConfig(
                gameIni,
                gameUserSettings);


            LoadOriginalConfigBase(
                gameIni,
                gameUserSettings);


            ApplyWeekendValues(
                gameIni,
                gameUserSettings);

            bool uploaded =
                await UploadConfigFilesAsync(
                    gameIni,
                    gameUserSettings);


            if (!uploaded)
            {
                return false;
            }

            OnLog?.Invoke(
                "✅ Event : Game.ini et GameUserSettings.ini disponibles");

            return true;
        }


        // =========================
        // SET CURRENT CONFIG AS ORIGINAL
        // =========================

        public async Task<bool> SetCurrentConfigAsOriginalAsync()
        {
            OnLog?.Invoke(
                "💾 Event : récupération de la configuration actuelle");


            string? gameIni =
                await DownloadConfigFileAsync(
                    GameIniPath,
                    "Game.ini");


            string? gameUserSettings =
                await DownloadConfigFileAsync(
                    GameUserSettingsPath,
                    "GameUserSettings.ini");


            if (gameIni == null ||
                gameUserSettings == null)
            {
                OnLog?.Invoke(
                    "❌ Event : impossible de définir la configuration d'origine");

                return false;
            }


            Directory.CreateDirectory(
                EventBackupPath);


            File.Copy(
                gameIni,
                GameIniBackupPath,
                true);


            File.Copy(
                gameUserSettings,
                GameUserSettingsBackupPath,
                true);


            OnLog?.Invoke(
                "✅ Event : configuration actuelle définie comme origine");


            return true;
        }


        // =========================
        // BACKUP ORIGINAL CONFIG
        // =========================

        private void BackupOriginalConfig(
            string gameIni,
            string gameUserSettings)
        {
            Directory.CreateDirectory(
                EventBackupPath);


            if (!File.Exists(GameIniBackupPath))
            {
                File.Copy(
                    gameIni,
                    GameIniBackupPath);
            }


            if (!File.Exists(GameUserSettingsBackupPath))
            {
                File.Copy(
                    gameUserSettings,
                    GameUserSettingsBackupPath);
            }


            OnLog?.Invoke(
                "💾 Event : configuration originale sauvegardée");
        }


        // =========================
        // RESTORE ORIGINAL CONFIG
        // =========================

        public async Task<bool> RestoreOriginalConfigAsync()
        {
            if (!File.Exists(GameIniBackupPath) ||
                !File.Exists(GameUserSettingsBackupPath))
            {
                OnLog?.Invoke(
                    "⚠ Event : aucune configuration originale à restaurer");

                return false;
            }


            OnLog?.Invoke(
                "🏁 Event : restauration de la configuration normale");


            bool uploaded =
                await UploadConfigFilesAsync(
                    GameIniBackupPath,
                    GameUserSettingsBackupPath);


            if (!uploaded)
            {
                OnLog?.Invoke(
                    "❌ Event : restauration impossible");

                return false;
            }


            OnLog?.Invoke(
                "✅ Event : configuration normale restaurée");

            File.Delete(
    GameIniBackupPath);

            File.Delete(
                GameUserSettingsBackupPath);


            OnLog?.Invoke(
                "🧹 Event : sauvegarde temporaire supprimée");

            return true;
        }


        // =========================
        // LOAD ORIGINAL CONFIG BASE
        // =========================

        private void LoadOriginalConfigBase(
            string gameIni,
            string gameUserSettings)
        {
            if (!HasOriginalConfigBackup)
                return;


            File.Copy(
                GameIniBackupPath,
                gameIni,
                true);


            File.Copy(
                GameUserSettingsBackupPath,
                gameUserSettings,
                true);


            OnLog?.Invoke(
                "📄 Event : configuration originale chargée comme base");
        }


        // =========================
        // APPLY WEEKEND VALUES
        // =========================

        private void ApplyWeekendValues(
            string gameIni,
            string gameUserSettings)
        {
            if (_config.WeekendTamingEnabled)
            {
                SetIniValue(
                    gameUserSettings,
                    "TamingSpeedMultiplier",
                    _config.WeekendTamingMultiplier.ToString(
                        System.Globalization.CultureInfo.InvariantCulture));
            }


            if (_config.WeekendHarvestEnabled)
            {
                SetIniValue(
                    gameUserSettings,
                    "HarvestAmountMultiplier",
                    _config.WeekendHarvestMultiplier.ToString(
                        System.Globalization.CultureInfo.InvariantCulture));
            }


            if (_config.WeekendXpEnabled)
            {
                SetIniValue(
                    gameIni,
                    "XPMultiplier",
                    _config.WeekendXpMultiplier.ToString(
                        System.Globalization.CultureInfo.InvariantCulture));
            }


            if (_config.WeekendWildDinoFoodDrainEnabled)
            {
                SetIniValue(
                    gameUserSettings,
                    "WildDinoCharacterFoodDrainMultiplier",
                    _config.WeekendWildDinoFoodDrainMultiplier.ToString(
                        System.Globalization.CultureInfo.InvariantCulture));
            }


            if (_config.WeekendBabyMatureEnabled)
            {
                SetIniValue(
                    gameIni,
                    "BabyMatureSpeedMultiplier",
                    _config.WeekendBabyMatureMultiplier.ToString(
                        System.Globalization.CultureInfo.InvariantCulture));
            }


            if (_config.WeekendBabyCuddleIntervalEnabled)
            {
                SetIniValue(
                    gameIni,
                    "BabyCuddleIntervalMultiplier",
                    _config.WeekendBabyCuddleIntervalMultiplier.ToString(
                        System.Globalization.CultureInfo.InvariantCulture));
            }


            OnLog?.Invoke(
                "🎉 Event : multiplicateurs week-end préparés");
        }



        // =========================
        // SET INI VALUE
        // =========================

        private void SetIniValue(
            string filePath,
            string key,
            string value)
        {
            string[] lines =
                File.ReadAllLines(filePath);


            bool found = false;


            for (int i = 0; i < lines.Length; i++)
            {
                string line =
                    lines[i].Trim();


                if (line.StartsWith(
                    key + "=",
                    StringComparison.OrdinalIgnoreCase))
                {
                    lines[i] =
                        $"{key}={value}";

                    found = true;

                    break;
                }
            }


            if (!found)
            {
                OnLog?.Invoke(
                    $"⚠ Event : paramètre introuvable - {key}");

                return;
            }


            File.WriteAllLines(
                filePath,
                lines);
        }


        // =========================
        // UPLOAD CONFIG FILES
        // =========================

        private async Task<bool> UploadConfigFilesAsync(
            string gameIni,
            string gameUserSettings)
        {
            bool gameIniUploaded =
                await _ftp.UploadFileAsync(
                    gameIni,
                    GameIniPath);


            bool gameUserSettingsUploaded =
                await _ftp.UploadFileAsync(
                    gameUserSettings,
                    GameUserSettingsPath);


            if (!gameIniUploaded ||
                !gameUserSettingsUploaded)
            {
                OnLog?.Invoke(
                    "❌ Event : upload des fichiers de configuration impossible");

                return false;
            }


            OnLog?.Invoke(
                "✅ Event : fichiers de configuration envoyés");

            return true;
        }

        // =========================
        // DOWNLOAD CONFIG FILE
        // =========================

        private async Task<string?> DownloadConfigFileAsync(
            string remotePath,
            string fileName)
        {
            string tempFolder =
                Path.Combine(
                    Path.GetTempPath(),
                    "ArkPilot",
                    "Events");


            Directory.CreateDirectory(
                tempFolder);


            string localPath =
                Path.Combine(
                    tempFolder,
                    fileName);


            bool success =
                await _ftp.DownloadFileAsync(
                    remotePath,
                    localPath);


            if (!success)
            {
                OnLog?.Invoke(
                    $"❌ Event : téléchargement impossible - {fileName}");

                return null;
            }


            return localPath;
        }
    }
}