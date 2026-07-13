using ArkPilot.Config;
using System;
using System.IO;
using System.Threading.Tasks;
using ArkPilot.Models;
using System.Globalization;

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

        private static readonly string UploadRollbackPath =
    Path.Combine(
        Path.GetTempPath(),
        "ArkPilot",
        "Events",
        "UploadRollback");


        private static readonly string RollbackGameIniPath =
            Path.Combine(
                UploadRollbackPath,
                "Game.ini");


        private static readonly string RollbackGameUserSettingsPath =
            Path.Combine(
                UploadRollbackPath,
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

            if (!HasOriginalConfigBackup)
            {
                OnLog?.Invoke(
                    "❌ Event : aucune configuration d'origine définie");

                return false;
            }


            bool originalValuesRestored =
                RestoreEventValuesFromOriginal(
                    gameIni,
                    gameUserSettings);


            if (!originalValuesRestored)
            {
                OnLog?.Invoke(
                    "❌ Event : restauration des valeurs événement impossible");

                return false;
            }


            bool valuesApplied =
                ApplyWeekendValues(
                    gameIni,
                    gameUserSettings);

            if (!valuesApplied)
            {
                OnLog?.Invoke(
                    "❌ Event : application de l'événement annulée");

                return false;
            }


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
        // RESTORE ORIGINAL CONFIG
        // =========================

        public async Task<bool> RestoreOriginalConfigAsync()
        {
            if (!HasOriginalConfigBackup)
            {
                OnLog?.Invoke(
                    "⚠ Event : aucune configuration originale à restaurer");

                return false;
            }


            OnLog?.Invoke(
                "🏁 Event : restauration de la configuration normale");


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


            bool originalValuesRestored =
                RestoreEventValuesFromOriginal(
                    gameIni,
                    gameUserSettings);


            if (!originalValuesRestored)
            {
                OnLog?.Invoke(
                    "❌ Event : restauration des valeurs d'origine impossible");

                return false;
            }


            bool uploaded =
                await UploadConfigFilesAsync(
                    gameIni,
                    gameUserSettings);


            if (!uploaded)
            {
                OnLog?.Invoke(
                    "❌ Event : restauration impossible");

                return false;
            }


            OnLog?.Invoke(
                "✅ Event : configuration normale restaurée");


            return true;
        }



        // =========================
        // GET ORIGINAL CONFIG
        // =========================

        public OriginalEventConfig? GetOriginalEventConfig()
        {
            if (!HasOriginalConfigBackup)
            {
                return null;
            }


            return new OriginalEventConfig
            {
                TamingMultiplier =
                    GetIniDoubleValue(
                        GameUserSettingsBackupPath,
                        "TamingSpeedMultiplier"),

                WildDinoFoodDrainMultiplier =
                    GetIniDoubleValue(
                        GameUserSettingsBackupPath,
                        "WildDinoCharacterFoodDrainMultiplier"),

                HarvestMultiplier =
                    GetIniDoubleValue(
                        GameUserSettingsBackupPath,
                        "HarvestAmountMultiplier"),

                BabyMatureMultiplier =
                    GetIniDoubleValue(
                        GameIniBackupPath,
                        "BabyMatureSpeedMultiplier"),

                BabyCuddleIntervalMultiplier =
                    GetIniDoubleValue(
                        GameIniBackupPath,
                        "BabyCuddleIntervalMultiplier"),

                XpMultiplier =
                    GetIniDoubleValue(
                        GameIniBackupPath,
                        "XPMultiplier")
            };
        }


        // =========================
        // GET INI DOUBLE VALUE
        // =========================

        private double GetIniDoubleValue(
            string filePath,
            string key)
        {
            string[] lines =
                File.ReadAllLines(filePath);


            foreach (string line in lines)
            {
                string value =
                    line.Trim();


                if (!value.StartsWith(
                    key + "=",
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }


                string rawValue =
                    value.Substring(
                        key.Length + 1);


                if (double.TryParse(
                    rawValue,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out double result))
                {
                    return result;
                }
            }


            return 0;
        }


        // =========================
        // RESTORE EVENT VALUES FROM ORIGINAL
        // =========================

        private bool RestoreEventValuesFromOriginal(
            string gameIni,
            string gameUserSettings)
        {
            OriginalEventConfig? original =
                GetOriginalEventConfig();


            if (original == null)
            {
                OnLog?.Invoke(
                    "❌ Event : configuration d'origine indisponible");

                return false;
            }


            bool success =
                true;


            success &=
                SetIniValue(
                    gameUserSettings,
                    "TamingSpeedMultiplier",
                    original.TamingMultiplier.ToString(
                        CultureInfo.InvariantCulture));


            success &=
                SetIniValue(
                    gameUserSettings,
                    "HarvestAmountMultiplier",
                    original.HarvestMultiplier.ToString(
                        CultureInfo.InvariantCulture));


            success &=
                SetIniValue(
                    gameIni,
                    "XPMultiplier",
                    original.XpMultiplier.ToString(
                        CultureInfo.InvariantCulture));


            success &=
                SetIniValue(
                    gameUserSettings,
                    "WildDinoCharacterFoodDrainMultiplier",
                    original.WildDinoFoodDrainMultiplier.ToString(
                        CultureInfo.InvariantCulture));


            success &=
                SetIniValue(
                    gameIni,
                    "BabyMatureSpeedMultiplier",
                    original.BabyMatureMultiplier.ToString(
                        CultureInfo.InvariantCulture));


            success &=
                SetIniValue(
                    gameIni,
                    "BabyCuddleIntervalMultiplier",
                    original.BabyCuddleIntervalMultiplier.ToString(
                        CultureInfo.InvariantCulture));


            if (!success)
            {
                OnLog?.Invoke(
                    "❌ Event : restauration des valeurs d'origine incomplète");

                return false;
            }


            OnLog?.Invoke(
                "📄 Event : valeurs événement restaurées depuis l'origine");


            return true;
        }


        // =========================
        // APPLY WEEKEND VALUES
        // =========================

        private bool ApplyWeekendValues(
            string gameIni,
            string gameUserSettings)
        {
            bool success =
                true;


            if (_config.WeekendTamingEnabled)
            {
                success &=
                    SetIniValue(
                        gameUserSettings,
                        "TamingSpeedMultiplier",
                        _config.WeekendTamingMultiplier.ToString(
                            CultureInfo.InvariantCulture));
            }


            if (_config.WeekendHarvestEnabled)
            {
                success &=
                    SetIniValue(
                        gameUserSettings,
                        "HarvestAmountMultiplier",
                        _config.WeekendHarvestMultiplier.ToString(
                            CultureInfo.InvariantCulture));
            }


            if (_config.WeekendXpEnabled)
            {
                success &=
                    SetIniValue(
                        gameIni,
                        "XPMultiplier",
                        _config.WeekendXpMultiplier.ToString(
                            CultureInfo.InvariantCulture));
            }


            if (_config.WeekendWildDinoFoodDrainEnabled)
            {
                success &=
                    SetIniValue(
                        gameUserSettings,
                        "WildDinoCharacterFoodDrainMultiplier",
                        _config.WeekendWildDinoFoodDrainMultiplier.ToString(
                            CultureInfo.InvariantCulture));
            }


            if (_config.WeekendBabyMatureEnabled)
            {
                success &=
                    SetIniValue(
                        gameIni,
                        "BabyMatureSpeedMultiplier",
                        _config.WeekendBabyMatureMultiplier.ToString(
                            CultureInfo.InvariantCulture));
            }


            if (_config.WeekendBabyCuddleIntervalEnabled)
            {
                success &=
                    SetIniValue(
                        gameIni,
                        "BabyCuddleIntervalMultiplier",
                        _config.WeekendBabyCuddleIntervalMultiplier.ToString(
                            CultureInfo.InvariantCulture));
            }


            if (!success)
            {
                OnLog?.Invoke(
                    "❌ Event : un ou plusieurs paramètres sont introuvables");

                return false;
            }


            OnLog?.Invoke(
                "🎉 Event : multiplicateurs week-end préparés");


            return true;
        }


        // =========================
        // SET INI VALUE
        // =========================

        private bool SetIniValue(
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

                return false;
            }


            File.WriteAllLines(
                filePath,
                lines);


            return true;
        }


        // =========================
        // UPLOAD FILE WITH RETRY
        // =========================

        private async Task<bool> UploadFileWithRetryAsync(
            string localPath,
            string remotePath)
        {
            bool success =
                await _ftp.UploadFileAsync(
                    localPath,
                    remotePath);


            if (success)
                return true;


            OnLog?.Invoke(
                $"⚠ Event : nouvelle tentative d'upload - {Path.GetFileName(localPath)}");


            await Task.Delay(
                TimeSpan.FromSeconds(2));


            return await _ftp.UploadFileAsync(
                localPath,
                remotePath);
        }


        // =========================
        // CLEAN UPLOAD ROLLBACK
        // =========================

        private void CleanUploadRollback()
        {
            try
            {
                if (Directory.Exists(
                    UploadRollbackPath))
                {
                    Directory.Delete(
                        UploadRollbackPath,
                        true);
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke(
                    $"⚠ Event : nettoyage rollback impossible - {ex.Message}");
            }
        }


        // =========================
        // BACKUP CURRENT SERVER CONFIG
        // =========================

        private async Task<bool> BackupCurrentServerConfigAsync()
        {
            Directory.CreateDirectory(
                UploadRollbackPath);


            bool gameIniDownloaded =
                await _ftp.DownloadFileAsync(
                    GameIniPath,
                    RollbackGameIniPath);


            bool gameUserSettingsDownloaded =
                await _ftp.DownloadFileAsync(
                    GameUserSettingsPath,
                    RollbackGameUserSettingsPath);


            if (!gameIniDownloaded ||
                !gameUserSettingsDownloaded)
            {
                OnLog?.Invoke(
                    "❌ Event : sauvegarde de secours avant upload impossible");

                return false;
            }


            OnLog?.Invoke(
                "💾 Event : sauvegarde de secours avant upload créée");


            return true;
        }

        // =========================
        // UPLOAD CONFIG FILES
        // =========================

        private async Task<bool> UploadConfigFilesAsync(
            string gameIni,
            string gameUserSettings)
        {
            bool backupCreated =
                await BackupCurrentServerConfigAsync();


            if (!backupCreated)
            {
                return false;
            }


            bool gameIniUploaded =
                await UploadFileWithRetryAsync(
                    gameIni,
                    GameIniPath);


            bool gameUserSettingsUploaded =
                await UploadFileWithRetryAsync(
                    gameUserSettings,
                    GameUserSettingsPath);


            if (!gameIniUploaded ||
                !gameUserSettingsUploaded)
            {
                OnLog?.Invoke(
                    "❌ Event : upload incomplet, restauration de secours");


                bool gameIniRestored =
                    await UploadFileWithRetryAsync(
                        RollbackGameIniPath,
                        GameIniPath);


                bool gameUserSettingsRestored =
                    await UploadFileWithRetryAsync(
                        RollbackGameUserSettingsPath,
                        GameUserSettingsPath);


                if (!gameIniRestored ||
                    !gameUserSettingsRestored)
                {
                    OnLog?.Invoke(
                        "❌ Event : restauration de secours incomplète");
                }
                else
                {
                    OnLog?.Invoke(
                        "✅ Event : configuration serveur restaurée après échec");
                }

                CleanUploadRollback();


                return false;
            }


            OnLog?.Invoke(
                "✅ Event : fichiers de configuration envoyés");

            CleanUploadRollback();


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