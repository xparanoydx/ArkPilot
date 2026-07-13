namespace ArkPilot.Models
{
    public class BackupFile
    {
        public string FileName { get; set; } = "";

        public string Date { get; set; } = "";

        public string Size { get; set; } = "";


        public override string ToString()
        {
            return $"{FileName}    {Date}    {Size}";
        }
    }
}