using ArkPilot.Models;
using ArkPilot.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArkPilot.Views
{
    public partial class DinosPage : Page
    {
        private readonly RconEngine rcon;


        private readonly ArkSaveDataService arkSaveDataService;

        public ObservableCollection<DinoSaveInfo> Dinos { get; }
            = new();

        private List<DinoSaveInfo> allDinos = new();


        public DinosPage(
            RconEngine engine,
            ArkSaveDataService saveDataService)
        {
            InitializeComponent();

            rcon = engine;

            arkSaveDataService = saveDataService;

            DinosGrid.ItemsSource = Dinos;

            Loaded +=
                DinosPage_Loaded;
        }


        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            string search = SearchBox.Text.Trim();

            Dinos.Clear();

            IEnumerable<DinoSaveInfo> filteredDinos = allDinos;

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredDinos = allDinos.Where(dino =>
                    dino.Species.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    dino.Name.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    dino.TribeName.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    dino.OwnerName.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase));
            }

            foreach (var dino in filteredDinos)
            {
                Dinos.Add(dino);

                UpdateCount();
            }
        }


        private void UpdateCount()
        {
            if (Dinos.Count == allDinos.Count)
            {
                CountText.Text = $"{Dinos.Count} dinos affichés";
            }
            else
            {
                CountText.Text =
                    $"{Dinos.Count} dinos affichés sur {allDinos.Count}";
            }
        }


        private void DinosGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DinosGrid.SelectedItem is not DinoSaveInfo dino)
                return;

            var window = new DinoDetailsWindow(dino)
            {
                Owner = Window.GetWindow(this)
            };

            window.ShowDialog();
        }



        private async void DinosPage_Loaded(
            object sender,
            System.Windows.RoutedEventArgs e)
        {
            var result =
                await arkSaveDataService.LoadDinosAsync();

            if (!result.Success)
            {
                LogService.Error(
                    $"SAVE DATA | Erreur dinos : {result.ErrorMessage}");

                return;
            }

            allDinos = result.Dinos;

            Dinos.Clear();

            foreach (var dino in allDinos)
            {
                Dinos.Add(dino);

                UpdateCount();
            }
        }
    }
}