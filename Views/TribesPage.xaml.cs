using ArkPilot.Models;
using ArkPilot.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Views
{
    public partial class TribesPage : Page
    {
        private readonly RconEngine rcon;

        public ObservableCollection<TribeInfo> Tribes { get; }
            = new();


        public TribesPage(RconEngine engine)
        {
            InitializeComponent();

            rcon = engine;

            TribesGrid.ItemsSource =
                Tribes;
        }


        // =========================
        // REFRESH TRIBES
        // =========================

        private void RefreshTribes_Click(
            object sender,
            RoutedEventArgs e)
        {
            TribeCountText.Text =
                $"🏰 Tribus : {Tribes.Count}";
        }
    }
}