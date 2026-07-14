using ArkPilot.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ArkPilot.Services
{
    public class BannedPlayerService
    {
        private static readonly string DataFolder =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ArkPilot");


        private static readonly string FileName =
            Path.Combine(
                DataFolder,
                "bannedplayers.json");


        public ObservableCollection<BannedPlayer> Players { get; }
            = new();


        public BannedPlayerService()
        {
            Directory.CreateDirectory(
                DataFolder);

            Load();
        }


        // =========================
        // ADD
        // =========================

        public void Add(
            string playerId,
            string playerName)
        {
            BannedPlayer? existingPlayer =
                Players.FirstOrDefault(
                    player => player.PlayerId == playerId);


            if (existingPlayer != null)
            {
                existingPlayer.PlayerName =
                    playerName;

                existingPlayer.BannedAt =
                    DateTime.Now;
            }
            else
            {
                Players.Add(
                    new BannedPlayer
                    {
                        PlayerId = playerId,
                        PlayerName = playerName,
                        BannedAt = DateTime.Now
                    });
            }


            Save();
        }


        // =========================
        // REMOVE
        // =========================

        public void Remove(
            string playerId)
        {
            BannedPlayer? player =
                Players.FirstOrDefault(
                    item => item.PlayerId == playerId);


            if (player == null)
                return;


            Players.Remove(
                player);


            Save();
        }


        // =========================
        // LOAD
        // =========================

        private void Load()
        {
            if (!File.Exists(FileName))
                return;


            try
            {
                string json =
                    File.ReadAllText(
                        FileName);


                var players =
                    JsonSerializer.Deserialize<
                        ObservableCollection<BannedPlayer>>(
                            json);


                if (players == null)
                    return;


                Players.Clear();


                foreach (var player in players)
                {
                    Players.Add(
                        player);
                }
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Joueurs bannis : {ex.Message}");
            }
        }


        // =========================
        // SAVE
        // =========================

        private void Save()
        {
            try
            {
                string json =
                    JsonSerializer.Serialize(
                        Players,
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
                    $"Joueurs bannis : {ex.Message}");
            }
        }
    }
}