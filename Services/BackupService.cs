using System;

namespace ArkPilot.Services
{
    public class BackupService
    {
        private readonly ArkService _ark;

        private readonly ArkSaveDataService _saveDataService;

        public DateTime LastBackupTime { get; private set; }

        public BackupService(
            ArkService ark,
            ArkSaveDataService saveDataService)
        {
            _ark = ark;
            _saveDataService = saveDataService;
        }

        public void SaveNow()
        {
            _ark.SaveWorld();

            _saveDataService.InvalidateCache();

            LastBackupTime = DateTime.Now;

            LogService.Success(
                $"SaveWorld envoyé à {LastBackupTime:HH:mm:ss}");
        }
    }
}