using ArkPilot.Models;
using ArkPilot.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArkPilot.Views
{
    public partial class BannedPlayersPage : Page
    {
        private readonly RconEngine rcon;

        private readonly BannedPlayerService bannedPlayers;

        private readonly AdminHistoryService adminHistory;


        public BannedPlayersPage(
            RconEngine rcon,
            BannedPlayerService bannedPlayers,
            AdminHistoryService adminHistory)
        {
            InitializeComponent();


            this.rcon =
                rcon;

            this.bannedPlayers =
                bannedPlayers;

            this.adminHistory =
                adminHistory;


            BannedPlayersGrid.ItemsSource =
                bannedPlayers.Players;


            UpdateCount();
        }


        // =========================
        // SEARCH
        // =========================

        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            string filter =
                SearchBox.Text?.Trim().ToLower() ?? "";


            if (string.IsNullOrWhiteSpace(filter))
            {
                BannedPlayersGrid.ItemsSource =
                    bannedPlayers.Players;

                UpdateCount();

                return;
            }


            BannedPlayersGrid.ItemsSource =
                new ObservableCollection<BannedPlayer>(
                    bannedPlayers.Players.Where(player =>
                        player.PlayerName
                            .ToLower()
                            .Contains(filter)
                        ||
                        player.PlayerId
                            .ToLower()
                            .Contains(filter)));


            UpdateCount();
        }


        // =========================
        // DOUBLE CLICK / UNBAN
        // =========================

        private void BannedPlayersGrid_MouseDoubleClick(
            object sender,
            MouseButtonEventArgs e)
        {
            if (BannedPlayersGrid.SelectedItem
                is not BannedPlayer player)
                return;


            var result =
                MessageBox.Show(
                    $"Voulez-vous vraiment débannir le joueur :\n\n{player.PlayerName} ?",
                    "Confirmation Déban",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
                return;


            LogService.Warning(
                $"ADMIN | Déban joueur : {player.PlayerName} ({player.PlayerId})");


            adminHistory.Add(
                "Déban",
                player.PlayerName,
                player.PlayerId);


            rcon.Send(
                $"UnBanPlayer {player.PlayerId}");


            bannedPlayers.Remove(
                player.PlayerId);


            BannedPlayersGrid.ItemsSource =
                bannedPlayers.Players;


            SearchBox.Clear();


            UpdateCount();
        }


        // =========================
        // COUNT
        // =========================

        private void UpdateCount()
        {
            BannedCountText.Text =
                $"🚫 Joueurs bannis : {BannedPlayersGrid.Items.Count}";
        }
    }
}