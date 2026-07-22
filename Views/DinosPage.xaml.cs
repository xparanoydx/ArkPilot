using ArkPilot.Helpers;
using ArkPilot.Models;
using ArkPilot.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;

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

            CreatureFilterBox.ItemsSource =
    ArkSpeciesHelper.GetFilterOptions();

            CreatureFilterBox.SelectedIndex = 0;

            rcon = engine;

            arkSaveDataService = saveDataService;

            DinosGrid.ItemsSource = Dinos;

            Loaded +=
                DinosPage_Loaded;
        }

        private void CreatureFilterBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (SearchBox == null ||
                CreatureFilterBox == null ||
                DinosGrid == null)
            {
                return;
            }

            string search = SearchBox.Text.Trim();

            IEnumerable<DinoSaveInfo> filteredDinos = allDinos;

            if (CreatureFilterBox.SelectedItem
                is CreatureFilterOption selectedOption &&
                selectedOption.Filter != CreatureFilter.All)
            {
                filteredDinos = filteredDinos.Where(dino =>
                {
                    CreatureInfo? creatureInfo =
                        ArkSpeciesHelper.GetCreatureInfo(dino.Species)
                        ?? ArkSpeciesHelper.GetCreatureInfo(dino.Name);

                    if (creatureInfo == null)
                    {
                        LogService.Warning(
                            $"DINO FILTER | Espèce non reconnue : {dino.Species}");

                        return false;
                    }

                    return selectedOption.Filter switch
                    {
                        CreatureFilter.Herbivores =>
                            creatureInfo.Category ==
                            CreatureCategory.Herbivore,

                        CreatureFilter.Carnivores =>
                            creatureInfo.Category ==
                            CreatureCategory.Carnivore,

                        CreatureFilter.Omnivores =>
                            creatureInfo.Category ==
                            CreatureCategory.Omnivore,

                        CreatureFilter.Flyers =>
                            creatureInfo.IsFlyer,

                        CreatureFilter.Aquatics =>
                            creatureInfo.IsAquatic,

                        CreatureFilter.Insects =>
                            creatureInfo.Category ==
                            CreatureCategory.Insect,

                        CreatureFilter.Fantasy =>
                            creatureInfo.IsFantasy,

                        CreatureFilter.Tek =>
                            creatureInfo.IsTek,

                        CreatureFilter.Bosses =>
                            creatureInfo.IsBoss,

                        CreatureFilter.Structure =>
                            creatureInfo.Category ==
                            CreatureCategory.Structure,

                        _ => true
                    };
                });
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredDinos = filteredDinos.Where(dino =>
                {
                    CreatureInfo? creatureInfo =
                        ArkSpeciesHelper.GetCreatureInfo(dino.Species);

                    bool matchesCreatureInfo =
                        creatureInfo != null &&
                        (
                            creatureInfo.Species.Contains(
                                search,
                                StringComparison.OrdinalIgnoreCase) ||

                            creatureInfo.DisplayName.Contains(
                                search,
                                StringComparison.OrdinalIgnoreCase) ||

                            creatureInfo.Aliases.Any(alias =>
                                alias.Contains(
                                    search,
                                    StringComparison.OrdinalIgnoreCase))
                        );

                    return
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
                            StringComparison.OrdinalIgnoreCase) ||

                        matchesCreatureInfo;
                });
            }


            Dinos.Clear();

            foreach (var dino in filteredDinos)
            {

                Dinos.Add(dino);
            }

            UpdateCount();
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

            ApplyFilters();

        }
    }
}