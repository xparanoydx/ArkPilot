using ArkPilot.Models;
using ArkPilot.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace ArkPilot.Views
{
    public partial class PlayersPage : Page
    {
        private readonly ServerMonitor? monitor;

        private readonly RconEngine rcon;

        private readonly ArkSaveDataService arkSaveDataService;

        private readonly PlayerNoteService noteService = new();

        private List<PlayerSaveInfo> savedPlayers =
            new();

        public ObservableCollection<PlayerInfo> Players { get; } = new();


        public PlayersPage(
            RconEngine engine,
            ArkSaveDataService saveDataService)
        {
            InitializeComponent();

            rcon = engine;

            arkSaveDataService = saveDataService;

            monitor =
                ((MainWindow)Application.Current.MainWindow)
                .Monitor;


            PlayersGrid.ItemsSource = Players;


            if (monitor != null)
            {
                monitor.Updated +=
                    Monitor_Updated;
            }

            Loaded +=
                PlayersPage_Loaded;

            Unloaded += (_, __) =>
            {
                if (monitor != null)
                {
                    monitor.Updated -=
                        Monitor_Updated;
                }
            };
        }


        private async void PlayersPage_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            var result =
                await arkSaveDataService.LoadPlayersAsync();

            if (result.Success)
            {
                savedPlayers =
                    result.Players;

            }
            else
            {
                LogService.Warning(
                    $"JOUEURS | {result.Error}");
            }

            UpdatePlayers();
        }



        // =========================
        // MONITOR UPDATE
        // =========================

        private void Monitor_Updated()
        {
            Dispatcher.Invoke(() =>
            {
                UpdatePlayers();
            });
        }



        private void UpdatePlayers()
        {
            Players.Clear();

            if (monitor == null)
            {
                Players.Clear();

                PlayerCountText.Text =
                    "👥 Joueurs connectés : 0";

                ServerStatus.Text =
                    "🔴 Serveur non initialisé";

                LastUpdate.Text =
                    "Dernière mise à jour : --";

                return;
            }


            foreach (var player in monitor.Players)
            {
                var note =
                    noteService.GetNote(
                        player.Id);


                player.Note =
                    note?.Note ?? "";


                string normalizedRconId =
                    player.Id
                        .Replace("-", "")
                        .Trim()
                        .ToUpperInvariant();


                var savedPlayer =
                    savedPlayers.FirstOrDefault(saved =>
                        !string.IsNullOrWhiteSpace(saved.UniqueNetId) &&
                        saved.UniqueNetId
                            .Replace("-", "")
                            .Trim()
                            .ToUpperInvariant() == normalizedRconId);


                if (savedPlayer != null)
                {
                    player.CharacterName =
                        savedPlayer.CharacterName;

                    player.Tribe =
                        savedPlayer.TribeName;

                    player.Level =
                        savedPlayer.Level;
                }

                Players.Add(player);
            }

            PlayerCountText.Text =
                $"👥 Joueurs connectés : {monitor.PlayerCount}";


            ServerStatus.Text =
                monitor.Online
                    ? "🟢 Serveur en ligne"
                    : "🔴 Serveur hors ligne";


            LastUpdate.Text =
                monitor.LastUpdate == DateTime.MinValue
                    ? "Dernière mise à jour : --"
                    : $"Dernière mise à jour : {monitor.LastUpdate:HH:mm:ss}";
        }



        // =========================
        // REFRESH BUTTON
        // =========================

        private void RefreshPlayersButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            UpdatePlayers();
        }



        // =========================
        // SEARCH
        // =========================

        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            string filter =
                SearchBox.Text?.ToLower() ?? "";


            if (string.IsNullOrWhiteSpace(filter))
            {
                PlayersGrid.ItemsSource = Players;
                return;
            }


            PlayersGrid.ItemsSource =
                new ObservableCollection<PlayerInfo>(
                    Players.Where(p =>
                        p.Name.ToLower()
                        .Contains(filter)
                        ||
                        p.Id.ToLower()
                        .Contains(filter)));
        }



        // =========================
        // PLAYER DETAILS
        // =========================

        private void PlayersGrid_MouseDoubleClick(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            if (PlayersGrid.SelectedItem
                is not PlayerInfo player)
                return;


            PlayerDetailsWindow window =
                new PlayerDetailsWindow(
                    player,
                    rcon,
                    noteService,
                    ((MainWindow)Application.Current.MainWindow)
                        .AdminHistory,
                    ((MainWindow)Application.Current.MainWindow)
                        .BannedPlayers)
                {
                    Owner =
                        Application.Current.MainWindow
                };


            window.ShowDialog();


            PlayersGrid.Items.Refresh();
        }
    }
}