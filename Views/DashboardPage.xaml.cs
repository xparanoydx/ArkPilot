using ArkPilot.Config;
using ArkPilot.Managers;
using ArkPilot.Models;
using ArkPilot.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArkPilot.Views
{
    public partial class DashboardPage : Page, IDisposable
    {
        private readonly RconEngine rcon;
        private readonly ServerMonitor monitor;
        private readonly BackupService backupService;
        private readonly DashboardManager dashboardManager;
        private readonly AutomationService automation;

        public DashboardPage(RconEngine engine)
        {
            InitializeComponent();

            rcon = engine;

            var mainWindow =
                (MainWindow)Application.Current.MainWindow;


            monitor =
                mainWindow.Monitor
                ?? throw new InvalidOperationException(
                    "Le moniteur serveur n'est pas initialisé.");


            backupService =
                mainWindow.BackupService;


            automation =
                mainWindow.Automation;

            var config = ConfigManager.Load();
            var nitradoService = new NitradoService(config);

            dashboardManager =
                new DashboardManager(
                    monitor,
                    backupService,
                    nitradoService);

            monitor.Updated += Monitor_Updated;

            automation.WeekendEventStateChanged +=
                Automation_WeekendEventStateChanged;

            Loaded += DashboardPage_Loaded;
            Unloaded += DashboardPage_Unloaded;
        }

        private async void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateWeekendEventBadge();

            await UpdateDashboardAsync();
        }

        private void DashboardPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private async void Monitor_Updated()
        {
            await UpdateDashboardAsync();
        }

        private async System.Threading.Tasks.Task UpdateDashboardAsync()
        {
            try
            {
                var info =
                    await dashboardManager.GetDashboardInfoAsync();

                if (!Dispatcher.CheckAccess())
                {
                    await Dispatcher.InvokeAsync(
                        () => ApplyDashboardInfo(info));

                    return;
                }

                ApplyDashboardInfo(info);
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Dashboard update impossible : {ex.Message}");

                if (!Dispatcher.HasShutdownStarted)
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        HeaderStatusText.Text =
                            "● ERREUR";

                        ApiCard.Value =
                            "Erreur";

                        ApiCard.Footer =
                            ex.Message;

                        ApiCard.AccentBrush =
                            (Brush)FindResource("BrushError");
                    });
                }
            }
        }

        private void ApplyDashboardInfo(
    DashboardInfo info)
        {
            if (info.Online)
            {
                HeaderStatusText.Text =
                    "● EN LIGNE";

                HeaderStatusText.Foreground =
                    Brushes.White;

                HeaderStatusBadge.Background =
                    Brushes.Transparent;

                HeaderStatusBadge.BorderBrush =
                    (Brush)FindResource("BrushSuccess");


                ServerCard.Icon = "🟢";
                ServerCard.Value = "EN LIGNE";

                ServerCard.AccentBrush =
                    (Brush)FindResource("BrushSuccess");
            }
            else
            {
                HeaderStatusText.Text =
                    "● HORS LIGNE";

                HeaderStatusText.Foreground =
                    Brushes.White;

                HeaderStatusBadge.Background =
                    Brushes.Transparent;

                HeaderStatusBadge.BorderBrush =
                    (Brush)FindResource("BrushError");


                ServerCard.Icon = "🔴";
                ServerCard.Value = "HORS LIGNE";

                ServerCard.AccentBrush =
                    (Brush)FindResource("BrushError");
            }

            ServerCard.Footer =
                monitor.LastUpdate == DateTime.MinValue
                    ? "Dernière MAJ : --"
                    : $"Dernière MAJ : {monitor.LastUpdate:HH:mm:ss}";

            PingCard.Value =
                info.Ping >= 0
                    ? $"{info.Ping} ms"
                    : "--";

            PlayersCard.Value =
                info.Players.ToString();

            UptimeCard.Value =
                info.Uptime;

            SaveCard.Value =
                info.LastBackup;

            RconCard.Value =
                rcon.IsConnected
                    ? "Connecté"
                    : "Déconnecté";

            RconCard.AccentBrush =
                rcon.IsConnected
                    ? (Brush)FindResource("BrushSuccess")
                    : (Brush)FindResource("BrushError");

            ApiCard.Value =
                info.ApiStatus;

            ApiCard.Footer =
                $"{info.Map} | " +
                $"{info.SlotsUsed}/{info.SlotsMax} | " +
                $"{info.Game}";

            ApiCard.AccentBrush =
                info.ApiStatus == "Started"
                    ? (Brush)FindResource("BrushSuccess")
                    : (Brush)FindResource("BrushWarning");
        }


        // =========================
        // WEEKEND EVENT STATE CHANGED
        // =========================

        private void Automation_WeekendEventStateChanged()
        {
            Dispatcher.Invoke(() =>
            {
                UpdateWeekendEventBadge();
            });
        }


        // =========================
        // UPDATE EVENT BADGE
        // =========================

        private void UpdateWeekendEventBadge()
        {
            WeekendEventBadge.Visibility =
                automation.WeekendEventActive == true
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }


        private async void SaveWorld_Click(object sender, RoutedEventArgs e)
        {
            backupService.SaveNow();

            await UpdateDashboardAsync();
        }

        private void DestroyDinos_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                "Détruire tous les dinos sauvages ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning)
                != MessageBoxResult.Yes)
            {
                return;
            }

            rcon.Send("DestroyWildDinos");
        }


        public void Dispose()
        {
            monitor.Updated -= Monitor_Updated;

            automation.WeekendEventStateChanged -=
                Automation_WeekendEventStateChanged;

            Loaded -= DashboardPage_Loaded;
            Unloaded -= DashboardPage_Unloaded;
        }
    }
}