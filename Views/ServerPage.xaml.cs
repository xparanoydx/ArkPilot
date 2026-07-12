using ArkPilot.Config;
using ArkPilot.Core;
using ArkPilot.Managers;
using ArkPilot.Services;
using System.Windows;
using System.Windows.Controls;
using System;

namespace ArkPilot.Views
{
    public partial class ServerPage : Page
    {
        private readonly RconEngine rcon;
        private readonly ServerMonitor monitor;
        private readonly ArkService ark;
        private readonly BackupService backupService;
        private readonly NitradoService nitradoService;
        private readonly DashboardManager dashboardManager;
        private readonly DialogService dialog = new();

        public ServerPage(RconEngine engine)
        {
            InitializeComponent();

            rcon = engine;

            monitor =
                ((MainWindow)Application.Current.MainWindow)
                .Monitor
                ?? throw new InvalidOperationException(
                    "ServerMonitor non initialisé.");

            var mainWindow =
                (MainWindow)Application.Current.MainWindow;

            ark = new ArkService(rcon);

            backupService =
                mainWindow.BackupService;

            var config = ConfigManager.Load();
            nitradoService = new NitradoService(config);

            dashboardManager =
                new DashboardManager(
                    monitor,
                    backupService,
                    nitradoService);

            Loaded += async (_, __) =>
            {
                await RefreshServerInfoAsync();
            };
        }

        private async System.Threading.Tasks.Task RefreshServerInfoAsync()
        {
            var info =
                await dashboardManager.GetDashboardInfoAsync();

            ServerStatusText.Text =
                $"État : {info.ApiStatus}";

            MapText.Text =
                $"Carte : {info.Map}";

            SlotsText.Text =
                $"Joueurs : {info.SlotsUsed} / {info.SlotsMax}";

            GameText.Text =
                $"Jeu : {info.Game}";
        }

        private async void Refresh_Click(
            object sender,
            RoutedEventArgs e)
        {
            await RefreshServerInfoAsync();
        }

        private void SaveWorld_Click(
            object sender,
            RoutedEventArgs e)
        {
            backupService.SaveNow();
        }

        private async void Broadcast_Click(
            object sender,
            RoutedEventArgs e)
        {
            string message =
                BroadcastBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
                return;

            string result =
                await ark.BroadcastAsync(message);

            if (result == "RCON_OFFLINE")
            {
                LogService.Error(
                    "Message impossible : RCON hors ligne");

                return;
            }

            if (result == "TIMEOUT" ||
                result == "RCON_ERROR")
            {
                LogService.Error(
                    "Échec de l'envoi du message serveur");

                return;
            }

            BroadcastBox.Clear();

            LogService.Success(
                $"Message serveur envoyé : {message}");
        }

        private void DestroyDinos_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!dialog.Confirm("Détruire tous les dinos sauvages ?"))
                return;

            rcon.Send("DestroyWildDinos");

            LogService.Warning("DestroyWildDinos envoyé");
        }

        private async void StartServer_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!dialog.Confirm("Démarrer le serveur via Nitrado ?"))
                return;

            await nitradoService.StartAsync();

            await RefreshServerInfoAsync();
        }

        private async void StopServer_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!dialog.Confirm("Arrêter le serveur via Nitrado ?"))
                return;

            await nitradoService.StopAsync();

            await RefreshServerInfoAsync();
        }

        private async void RestartServer_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!dialog.Confirm("Redémarrer le serveur via Nitrado ?"))
                return;

            await nitradoService.RestartAsync();

            await RefreshServerInfoAsync();
        }
    }
}