using ArkPilot.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace ArkPilot.Services
{
    public class AdminHistoryService
    {
        private static readonly string DataFolder =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ArkPilot");


        private static readonly string FileName =
            Path.Combine(
                DataFolder,
                "adminhistory.json");

        public ObservableCollection<AdminHistoryEntry> History { get; }
            = new();


        public AdminHistoryService()
        {
            Directory.CreateDirectory(
                DataFolder);

            Load();
        }

        public void Add(
            string action,
            string target,
            string details)
        {
            History.Add(
                new AdminHistoryEntry
                {
                    Timestamp = DateTime.Now,
                    Action = action,
                    Target = target,
                    Details = details
                });


            while (History.Count > 500)
            {
                History.RemoveAt(0);
            }


            Save();
        }


        public void Clear()
        {
            History.Clear();

            Save();
        }


        private void Load()
        {
            if (!File.Exists(FileName))
                return;


            try
            {
                string json =
                    File.ReadAllText(FileName);


                var entries =
                    JsonSerializer.Deserialize<
                        ObservableCollection<AdminHistoryEntry>>(
                            json);


                if (entries == null)
                    return;


                History.Clear();


                foreach (var entry in entries)
                {
                    History.Add(entry);
                }


                while (History.Count > 500)
                {
                    History.RemoveAt(0);
                }
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Historique administration : {ex.Message}");
            }
        }


        private void Save()
        {
            try
            {
                string json =
                    JsonSerializer.Serialize(
                        History,
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
                    $"Historique administration : {ex.Message}");
            }
        }
    }
}