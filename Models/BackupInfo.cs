using ArkPilot.Helpers;
using System;

namespace ArkPilot.Models
{
    public class BackupInfo
    {
        public string FileName { get; set; } = "";

        public string FullPath { get; set; } = "";

        public DateTime LastWriteTime { get; set; }

        public long Size { get; set; }

        public string DateText =>
            LastWriteTime.ToString("dd/MM/yyyy HH:mm");

        public string SizeText =>
            FileSizeHelper.Format(Size);

    }
}