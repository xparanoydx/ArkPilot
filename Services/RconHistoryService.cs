using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ArkPilot.Services
{
    public class RconHistoryService
    {
        private static readonly string DataFolder =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ArkPilot");


        private static readonly string FileName =
            Path.Combine(
                DataFolder,
                "rconhistory.json");

        public List<RconHistoryEntry> Load()
        {
            if (!File.Exists(FileName))
            {
                return new List<RconHistoryEntry>();
            }


            try
            {
                string json =
                    File.ReadAllText(FileName);


                return
                    JsonSerializer.Deserialize<List<RconHistoryEntry>>(
                        json)
                    ?? new List<RconHistoryEntry>();
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Historique RCON : {ex.Message}");

                return new List<RconHistoryEntry>();
            }
        }

        public RconHistoryService()
        {
            Directory.CreateDirectory(
                DataFolder);
        }


        public void Save(
            IEnumerable<RconHistoryEntry> history)
        {
            try
            {
                string json =
                    JsonSerializer.Serialize(
                        history,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });


                File.WriteAllText(
                    FileName,
                    json);
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Historique RCON : {ex.Message}");
            }
        }
    }
}