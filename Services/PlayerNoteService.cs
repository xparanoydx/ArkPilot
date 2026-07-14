using ArkPilot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ArkPilot.Services
{
    public class PlayerNoteService
    {
        private static readonly string DataFolder =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ArkPilot");


        private static readonly string FileName =
            Path.Combine(
                DataFolder,
                "playernotes.json");

        private readonly List<PlayerNote> notes = new();


        public PlayerNoteService()
        {
            Directory.CreateDirectory(
                DataFolder);

            Load();
        }

        public PlayerNote? GetNote(
            string playerId)
        {
            return notes.FirstOrDefault(
                n => n.PlayerId == playerId);
        }


        public void SaveNote(
            string playerId,
            string playerName,
            string note)
        {
            PlayerNote? playerNote =
                GetNote(playerId);


            if (playerNote == null)
            {
                playerNote =
                    new PlayerNote
                    {
                        PlayerId = playerId
                    };

                notes.Add(playerNote);
            }


            playerNote.PlayerName =
                playerName;

            playerNote.Note =
                note;

            playerNote.UpdatedAt =
                DateTime.Now;


            Save();
        }


        public void DeleteNote(
            string playerId)
        {
            PlayerNote? playerNote =
                GetNote(playerId);


            if (playerNote == null)
                return;


            notes.Remove(
                playerNote);


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


                List<PlayerNote>? loadedNotes =
                    JsonSerializer.Deserialize<List<PlayerNote>>(
                        json);


                if (loadedNotes != null)
                {
                    notes.Clear();

                    notes.AddRange(
                        loadedNotes);
                }
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Notes joueurs : {ex.Message}");
            }
        }


        private void Save()
        {
            try
            {
                string json =
                    JsonSerializer.Serialize(
                        notes,
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
                    $"Notes joueurs : {ex.Message}");
            }
        }
    }
}