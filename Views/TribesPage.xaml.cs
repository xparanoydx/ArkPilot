using ArkPilot.Models;
using ArkPilot.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ArkPilot.Views
{
    public partial class TribesPage : Page
    {
        private readonly RconEngine rcon;

        private readonly ArkSaveDataService arkSaveDataService;

        public ObservableCollection<TribeInfo> Tribes { get; }
            = new();

        private readonly DispatcherTimer syncTimer =
            new();

        private bool isRefreshing;

        private bool hasLoadedOnce;


        public TribesPage(
            RconEngine engine,
            ArkSaveDataService saveDataService)
        {
            InitializeComponent();

            rcon = engine;

            arkSaveDataService = saveDataService;
            
            TribesGrid.ItemsSource =
                Tribes;

            syncTimer.Interval =
                TimeSpan.FromMinutes(5);

            syncTimer.Tick +=
                SyncTimer_Tick;

            syncTimer.Start();

            Loaded +=
                TribesPage_Loaded;

            Unloaded +=
                TribesPage_Unloaded;
        }


        private void TribesGrid_MouseDoubleClick(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TribesGrid.SelectedItem is not TribeInfo tribe)
            {
                return;
            }


            var window =
                new TribeDetailsWindow(
                    tribe)
                {
                    Owner =
                        Application.Current.MainWindow
                };


            window.ShowDialog();
        }


        private async void TribesPage_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            syncTimer.Start();

            if (!hasLoadedOnce)
            {
                hasLoadedOnce = true;

                await RefreshTribesAsync();
            }
        }



        private async void SyncTimer_Tick(
            object? sender,
                EventArgs e)
        {
            if (!IsVisible)
            {
                return;
            }

            await RefreshTribesAsync();
        }


        private void TribesPage_Unloaded(
            object sender,
            RoutedEventArgs e)
        {
            syncTimer.Stop();
        }


        // =========================
        // REFRESH TRIBES
        // =========================

        private async Task RefreshTribesAsync()
        {

            if (isRefreshing)
            {
                return;
            }

            isRefreshing = true;

            try
            {
                var result =
                    await arkSaveDataService.LoadTribesAsync();

                if (!result.Success)
                {
                    LogService.Warning(
                        $"TRIBUS | {result.Error}");

                    return;
                }

                Tribes.Clear();

                foreach (var tribe in result.Tribes)
                {
                    Tribes.Add(tribe);
                }

                TribeCountText.Text =
                    $"🏰 Tribus : {Tribes.Count}";
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"TRIBUS | Erreur chargement : {ex}");
            }

            finally
            {
                isRefreshing = false;
            }
        }

        private async void RefreshTribes_Click(
            object sender,
            RoutedEventArgs e)
        {
            await RefreshTribesAsync();
        }
    }
}