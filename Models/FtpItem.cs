using System;

namespace ArkPilot.Models
{
    public class FtpItem
    {
        public string Name { get; set; } = "";

        public string FullPath { get; set; } = "";

        public bool IsDirectory { get; set; }

        public long Size { get; set; }

        public DateTime Modified { get; set; }

        public string SizeText
        {
            get
            {
                if (IsDirectory)
                    return "";

                const double KB = 1024;
                const double MB = KB * 1024;
                const double GB = MB * 1024;

                if (Size >= GB)
                    return $"{Size / GB:0.0} GB";

                if (Size >= MB)
                    return $"{Size / MB:0.0} MB";

                if (Size >= KB)
                    return $"{Size / KB:0.0} KB";

                return $"{Size} B";
            }
        }

        public string ModifiedText =>
    Modified == default
        ? ""
        : Modified.ToString("dd/MM/yyyy HH:mm");

        public string Icon
        {
            get
            {
                if (IsDirectory)
                    return "📁";

                string ext =
                    System.IO.Path.GetExtension(Name).ToLower();

                return ext switch
                {
                    ".ark" => "🦖",
                    ".arktribe" => "👥",
                    ".arkprofile" => "👤",
                    ".ini" => "⚙",
                    ".log" => "📜",
                    ".json" => "📋",
                    ".zip" => "📦",
                    ".bak" => "💾",
                    _ => "📄"
                };
            }
        }

        public override string ToString()
        {
            return IsDirectory
                ? $"📁 {Name}"
                : $"📄 {Name}";
        }
    }
}