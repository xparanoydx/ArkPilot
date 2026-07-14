using ArkPilot.Models;
using ArkPilot.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Views
{
    public partial class AdminHistoryPage : Page
    {
        private readonly AdminHistoryService adminHistory;


        public AdminHistoryPage(
            AdminHistoryService adminHistory)
        {
            InitializeComponent();


            this.adminHistory =
                adminHistory;


            HistoryGrid.ItemsSource =
                adminHistory.History;


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
                HistoryGrid.ItemsSource =
                    adminHistory.History;

                UpdateCount();

                return;
            }


            HistoryGrid.ItemsSource =
                new ObservableCollection<AdminHistoryEntry>(
                    adminHistory.History.Where(entry =>
                        entry.Action.ToLower().Contains(filter)
                        ||
                        entry.Target.ToLower().Contains(filter)
                        ||
                        entry.Details.ToLower().Contains(filter)));


            UpdateCount();
        }


        // =========================
        // CLEAR HISTORY
        // =========================

        private void ClearHistory_Click(
            object sender,
            RoutedEventArgs e)
        {
            var result =
                MessageBox.Show(
                    "Voulez-vous vraiment effacer tout l'historique d'administration ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
                return;


            adminHistory.Clear();


            HistoryGrid.ItemsSource =
                adminHistory.History;


            SearchBox.Clear();


            UpdateCount();
        }


        // =========================
        // COUNT
        // =========================

        private void UpdateCount()
        {
            HistoryCountText.Text =
                $"📜 Actions : {HistoryGrid.Items.Count}";
        }
    }
}