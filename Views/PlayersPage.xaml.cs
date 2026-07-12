using ArkPilot.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Views
{
    public partial class PlayersPage : Page
    {
        private readonly ServerMonitor? monitor;
        private readonly RconEngine rcon;

        public ObservableCollection<PlayerInfo> Players { get; } = new();


        public PlayersPage(RconEngine engine)
        {
            InitializeComponent();

            rcon = engine;

            monitor =
                ((MainWindow)Application.Current.MainWindow)
                .Monitor;


            PlayersGrid.ItemsSource = Players;


            if (monitor != null)
            {
                monitor.Updated +=
                    Monitor_Updated;
            }

            Loaded += (_, __) =>
            {
                UpdatePlayers();
            };


            Unloaded += (_, __) =>
            {
                if (monitor != null)
                {
                    monitor.Updated -=
                        Monitor_Updated;
                }
            };
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
        // KICK
        // =========================

        private void KickButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (PlayersGrid.SelectedItem
                is not PlayerInfo player)
                return;


            rcon.Send(
                $"KickPlayer {player.Id}");
        }



        // =========================
        // BAN
        // =========================

        private void BanButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (PlayersGrid.SelectedItem
                is not PlayerInfo player)
                return;


            rcon.Send(
                $"BanPlayer {player.Id}");
        }




        // =========================
        // COPY ID
        // =========================

        private void CopyId_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (PlayersGrid.SelectedItem
                is PlayerInfo player)
            {
                Clipboard.SetText(
                    player.Id);
            }
        }
    }
}