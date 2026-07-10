using System;

namespace ArkPilot.Services
{
    public class BackupService
    {
        private readonly ArkService _ark;

        public DateTime LastBackupTime { get; private set; }

        public BackupService(ArkService ark)
        {
            _ark = ark;
        }

        public void SaveNow()
        {
            _ark.SaveWorld();

            LastBackupTime = DateTime.Now;

            LogService.Success(
                $"SaveWorld envoyé à {LastBackupTime:HH:mm:ss}");
        }
    }
}