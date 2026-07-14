using ArkPilot.Services;
using ArkPilot.Views;
using ArkPilot.Config;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using ArkPilot.Helpers;
using ArkPilot.Core;
using System.Threading.Tasks;


namespace ArkPilot
{
    public partial class MainWindow : Window
    {
        private readonly RconClient rawRcon = new();
        private readonly RconEngine rcon;
        private readonly AutomationService _automation;

        private readonly AdminHistoryService adminHistory =
            new();

        private readonly BannedPlayerService bannedPlayers =
            new();

        public RconEngine Rcon => rcon;

        public AutomationService Automation =>
            _automation;

        public ServerMonitor? Monitor => monitor;

        private readonly ArkService ark;

        private readonly BackupService backupService;

        public BackupService BackupService =>
            backupService;

        public AdminHistoryService AdminHistory =>
            adminHistory;

        public BannedPlayerService BannedPlayers =>
            bannedPlayers;


        private NavigationService navigation;

        private bool logsExpanded;

        private bool playersMenuExpanded;

        private ServerMonitor? monitor;

        private readonly DispatcherTimer statusTimer = new();


        public MainWindow()
        {
            InitializeComponent();

            navigation = new NavigationService(MainContent);


            // =========================
            // RCON ENGINE
            // =========================

            rcon = new RconEngine(rawRcon);

            ark = new ArkService(rcon);

            backupService =
                new BackupService(ark);

            _automation = new AutomationService(
                rcon,
                ConfigManager.Load());

            // =========================
            // LOG SYSTEM
            // =========================

            void AddLog(string msg)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    LogList.Items.Add(msg);

                    if (LogList.Items.Count > AppConstants.MaxLogLines)
                        LogList.Items.RemoveAt(0);

                    LogList.ScrollIntoView(
                        LogList.Items[^1]);
                });
            }
        
            LogService.OnLog += AddLog;

            _automation.OnLog += AddLog;

            // =========================
            // CLOCK
            // =========================

           statusTimer.Interval =
                TimeSpan.FromSeconds(1);


            statusTimer.Tick +=
                StatusTimer_Tick;


            statusTimer.Start();



            Loaded +=
                MainWindow_Loaded;
        }





        // =========================
        // STARTUP
        // =========================

        private async void MainWindow_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            ServerConfig config =
                ConfigManager.Load();


            if (string.IsNullOrWhiteSpace(config.ServerIp) ||
                string.IsNullOrWhiteSpace(config.RconPassword))
            {
                StatusRcon.Text =
                    "🟡 Configuration requise";


                navigation.Navigate(
                    new SettingsPage(
                        rcon,
                        _automation));


                return;
            }


            bool connected =
                await InitializeServerAsync();


            StatusRcon.Text =
                connected
                ? "🟢 RCON : Connecté"
                : "🔴 RCON : Déconnecté";


            StatusNitrado.Text =
                connected
                ? "🟢 Serveur prêt"
                : "🟠 Vérifiez la configuration";


            navigation.Navigate(
                connected
                ? new DashboardPage(rcon)
                : new SettingsPage(
                    rcon,
                    _automation));
        }

        // =========================
        // INITIALIZE SERVER
        // =========================

        public async Task<bool> InitializeServerAsync()
        {
            ServerConfig config =
                ConfigManager.Load();


            if (string.IsNullOrWhiteSpace(config.ServerIp) ||
                string.IsNullOrWhiteSpace(config.RconPassword))
            {
                return false;
            }


            if (monitor == null)
            {
                monitor =
                    new ServerMonitor(
                        rcon,
                        config.ServerIp,
                        config.RconPort,
                        config.RconPassword);


                monitor.Updated +=
                    Monitor_Updated;
            }


            bool connected =
                await rcon.Connect(
                    config.ServerIp,
                    config.RconPort,
                    config.RconPassword);


            if (connected)
            {
                monitor.Start();

                _automation.Start();
            }


            return connected;
        }





        // =========================
        // MONITOR STATUS
        // =========================

        private void Monitor_Updated()
        {
            ServerMonitor? currentMonitor =
                monitor;


            if (currentMonitor == null)
                return;


            Dispatcher.Invoke(() =>
            {
                StatusPlayers.Text =
                    $"👥 {currentMonitor.PlayerCount} joueur(s)";


                PingText.Text =
                    currentMonitor.Ping >= 0
                    ? $"Ping : {currentMonitor.Ping} ms"
                    : "Ping : --";


                StatusRcon.Text =
                    currentMonitor.Online
                    ? "🟢 Serveur connecté"
                    : "🔴 Serveur hors ligne";
            });
        }




        // =========================
        // CLOCK
        // =========================

        private void StatusTimer_Tick(
            object? sender,
            EventArgs e)
        {
            StatusClock.Text =
                $"🕒 {DateTime.Now:HH:mm:ss}";
        }



        // =========================
        // INITIALISATION MONITOR
        // =========================

        private async Task<bool> EnsureMonitorAsync()
        {
            if (monitor != null)
                return true;

            var config =
                ConfigManager.Load();

            if (string.IsNullOrWhiteSpace(config.ServerIp) ||
                string.IsNullOrWhiteSpace(config.RconPassword))
            {
                StatusRcon.Text =
                    "🟡 Configuration requise";

                return false;
            }

            monitor =
                new ServerMonitor(
                    rcon,
                    config.ServerIp,
                    config.RconPort,
                    config.RconPassword);

            monitor.Updated +=
                Monitor_Updated;

            bool connected =
                await rcon.Connect(
                    config.ServerIp,
                    config.RconPort,
                    config.RconPassword);

            if (connected)
            {
                monitor.Start();
            }

            StatusRcon.Text =
                connected
                    ? "🟢 RCON : Connecté"
                    : "🔴 RCON : Déconnecté";

            StatusNitrado.Text =
                connected
                    ? "🟢 Serveur prêt"
                    : "🟠 Vérifiez la configuration";

            return true;
        }




        // =========================
        // NAVIGATION
        // =========================

        private void Console_Click(
            object sender,
            RoutedEventArgs e)
        {
            navigation.Navigate(
                new ConsolePage(rcon));
        }

        private void Chat_Click(
            object sender,
            RoutedEventArgs e)
        {
            navigation.Navigate(
                new ChatPage(
                    rcon,
                    adminHistory));
        }


        private async void Dashboard_Click(
            object sender,
            RoutedEventArgs e)
        {
            bool ready =
                await EnsureMonitorAsync();

            if (!ready)
            {
                navigation.Navigate(
                    new SettingsPage(
                        rcon,
                        _automation));

                return;
            }

            navigation.Navigate(
                new DashboardPage(rcon));
        }

        private void TogglePlayersMenu_Click(
            object sender,
            RoutedEventArgs e)
        {
            playersMenuExpanded =
                !playersMenuExpanded;


            PlayersSubMenu.Visibility =
                playersMenuExpanded
                    ? Visibility.Visible
                    : Visibility.Collapsed;


            PlayersMenuButton.Content =
                playersMenuExpanded
                    ? "🛡 Administration joueurs  ▼"
                    : "🛡 Administration joueurs  ▶";
        }

        private void Players_Click(
    object sender,
    RoutedEventArgs e)
        {
            navigation.Navigate(
                new PlayersPage(rcon));
        }

        private void BannedPlayers_Click(
    object sender,
    RoutedEventArgs e)
        {
            navigation.Navigate(
                new BannedPlayersPage(
                    rcon,
                    bannedPlayers,
                    adminHistory));
        }

        private void Tribes_Click(
    object sender,
    RoutedEventArgs e)
        {
            navigation.Navigate(
                new TribesPage(rcon));
        }

        private void AdminHistory_Click(
    object sender,
    RoutedEventArgs e)
        {
            navigation.Navigate(
                new AdminHistoryPage(
                    adminHistory));
        }


        private void Settings_Click(
            object sender,
            RoutedEventArgs e)
        {
            navigation.Navigate(
                new SettingsPage(
                 rcon,
                  _automation));
        }



        private async void Server_Click(
            object sender,
            RoutedEventArgs e)
        {
            bool ready =
                await EnsureMonitorAsync();


            if (!ready)
            {
                navigation.Navigate(
                    new SettingsPage(
                        rcon,
                        _automation));

                return;
            }


            navigation.Navigate(
                new ServerPage(rcon));
        }

        private void Backups_Click(
    object sender,
    RoutedEventArgs e)
        {
            navigation.Navigate(
                new BackupsPage());
        }

        private void Events_Click(
    object sender,
    RoutedEventArgs e)
        {
            navigation.Navigate(
                new EventsPage(
                    _automation));
        }


        private void FtpExplorer_Click(
    object sender,
    RoutedEventArgs e)
        {
            navigation.Navigate(
                new FtpExplorerPage());
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow
            {
                Owner = this
            };

            window.ShowDialog();
        }


        // =========================
        // TOGGLE LOGS
        // =========================

        private void ToggleLogs_Click(
            object sender,
            RoutedEventArgs e)
        {
            logsExpanded =
                !logsExpanded;


            if (logsExpanded)
            {
                LogRow.Height =
                    new GridLength(450);

                ToggleLogsButton.Content =
                    "⛶ Réduire les logs";
            }
            else
            {
                LogRow.Height =
                    new GridLength(160);

                ToggleLogsButton.Content =
                    "⛶ Agrandir les logs";
            }
        }



        // =========================
        // CLOSE
        // =========================

        protected override void OnClosing(
            CancelEventArgs e)
        {
            statusTimer.Stop();

            monitor?.Stop();

            _automation.Stop();

            rcon.Stop();


            base.OnClosing(e);
        }


        protected override void OnClosed(
            EventArgs e)
        {
            try
            {
                monitor?.Stop();

                rcon.Dispose();

                ProcessCleaner.KillGhostProcesses();
            }
            catch
            {
            }


            base.OnClosed(e);
        }
    }
}