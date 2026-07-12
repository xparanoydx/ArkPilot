using ArkPilot.Services;
using System.Windows.Controls;
using ArkPilot.Config;
using System;
using System.Windows;
using System.Windows.Media;
using System.Globalization;

namespace ArkPilot.Views
{
    public partial class EventsPage : Page
    {
        private readonly AutomationService automation;

        private ServerConfig config;


        public EventsPage(
            AutomationService automationService)
        {
            InitializeComponent();

            automation = automationService;

            config = ConfigManager.Load();

            LoadConfig();
        }


        // =========================
        // CHARGEMENT CONFIG
        // =========================

        private void LoadConfig()
        {
            WeekendEventBox.IsChecked =
                config.WeekendEventEnabled;

            WeekendPeriodText.Text =
                $"Début : {GetDayName(config.WeekendStartDay)} " +
                $"{config.WeekendStartHour:D2}:{config.WeekendStartMinute:D2}\n" +
                $"Fin : {GetDayName(config.WeekendEndDay)} " +
                $"{config.WeekendEndHour:D2}:{config.WeekendEndMinute:D2}";

            WeekendStartDayBox.SelectedIndex =
                config.WeekendStartDay;

            WeekendStartHourBox.Text =
                config.WeekendStartHour.ToString("D2");

            WeekendStartMinuteBox.Text =
                config.WeekendStartMinute.ToString("D2");


            WeekendEndDayBox.SelectedIndex =
                config.WeekendEndDay;

            WeekendEndHourBox.Text =
                config.WeekendEndHour.ToString("D2");

            WeekendEndMinuteBox.Text =
                config.WeekendEndMinute.ToString("D2");

            WeekendOptionsPanel.IsEnabled =
                config.WeekendEventEnabled;

            WeekendTamingEnabledBox.IsChecked =
                config.WeekendTamingEnabled;

            WeekendTamingBox.Text =
                config.WeekendTamingMultiplier.ToString();

            WeekendWildDinoFoodDrainEnabledBox.IsChecked =
                config.WeekendWildDinoFoodDrainEnabled;

            WeekendWildDinoFoodDrainBox.Text =
                config.WeekendWildDinoFoodDrainMultiplier.ToString();

            WeekendHarvestEnabledBox.IsChecked =
                config.WeekendHarvestEnabled;

            WeekendHarvestBox.Text =
                config.WeekendHarvestMultiplier.ToString();

            WeekendBabyMatureEnabledBox.IsChecked =
                config.WeekendBabyMatureEnabled;

            WeekendBabyMatureBox.Text =
                config.WeekendBabyMatureMultiplier.ToString();

            WeekendBabyCuddleIntervalEnabledBox.IsChecked =
                config.WeekendBabyCuddleIntervalEnabled;

            WeekendBabyCuddleIntervalBox.Text =
                config.WeekendBabyCuddleIntervalMultiplier.ToString();

            WeekendXpEnabledBox.IsChecked =
                config.WeekendXpEnabled;

            WeekendXpBox.Text =
                config.WeekendXpMultiplier.ToString();


            StatusText.Text =
                "⚪ Configuration chargée";
        }


        // =========================
        // SAVE EVENT CLICK 
        // =========================


        private void SaveEvent_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                ServerConfig config =
                    ConfigManager.Load();


                bool tamingEnabled =
                    WeekendTamingEnabledBox.IsChecked == true;

                bool harvestEnabled =
                    WeekendHarvestEnabledBox.IsChecked == true;

                bool babyMatureEnabled =
                    WeekendBabyMatureEnabledBox.IsChecked == true;

                bool xpEnabled =
                    WeekendXpEnabledBox.IsChecked == true;

                bool wildDinoFoodDrainEnabled =
                    WeekendWildDinoFoodDrainEnabledBox.IsChecked == true;

                bool babyCuddleIntervalEnabled =
                    WeekendBabyCuddleIntervalEnabledBox.IsChecked == true;


                double taming =
                    config.WeekendTamingMultiplier;

                double harvest =
                    config.WeekendHarvestMultiplier;

                double babyMature =
                    config.WeekendBabyMatureMultiplier;

                double xp =
                    config.WeekendXpMultiplier;

                double wildDinoFoodDrain =
                    config.WeekendWildDinoFoodDrainMultiplier;

                double babyCuddleInterval =
                    config.WeekendBabyCuddleIntervalMultiplier;


                if (tamingEnabled &&
                    !double.TryParse(
                        WeekendTamingBox.Text.Replace(',', '.'),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out taming))
                {
                    StatusError(
                        "Multiplicateur Taming invalide");

                    return;
                }


                if (harvestEnabled &&
                    !double.TryParse(
                        WeekendHarvestBox.Text.Replace(',', '.'),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out harvest))
                {
                    StatusError(
                        "Multiplicateur Récolte invalide");

                    return;
                }


                if (babyMatureEnabled &&
                    !double.TryParse(
                        WeekendBabyMatureBox.Text.Replace(',', '.'),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out babyMature))
                {
                    StatusError(
                        "Multiplicateur croissance invalide");

                    return;
                }


                if (xpEnabled &&
                    !double.TryParse(
                        WeekendXpBox.Text.Replace(',', '.'),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out xp))
                {
                    StatusError(
                        "Multiplicateur XP invalide");

                    return;
                }


                if (wildDinoFoodDrainEnabled &&
                    !double.TryParse(
                        WeekendWildDinoFoodDrainBox.Text.Replace(',', '.'),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out wildDinoFoodDrain))
                {
                    StatusError(
                        "Wild Dino Food Drain invalide");

                    return;
                }


                if (babyCuddleIntervalEnabled &&
                    !double.TryParse(
                        WeekendBabyCuddleIntervalBox.Text.Replace(',', '.'),
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out babyCuddleInterval))
                {
                    StatusError(
                        "Intervalle câlins bébés invalide");

                    return;
                }


                config.WeekendEventEnabled =
                    WeekendEventBox.IsChecked == true;


                config.WeekendTamingEnabled =
                    tamingEnabled;

                config.WeekendHarvestEnabled =
                    harvestEnabled;

                config.WeekendBabyMatureEnabled =
                    babyMatureEnabled;

                config.WeekendXpEnabled =
                    xpEnabled;

                config.WeekendWildDinoFoodDrainEnabled =
                    wildDinoFoodDrainEnabled;

                config.WeekendBabyCuddleIntervalEnabled =
                    babyCuddleIntervalEnabled;


                config.WeekendTamingMultiplier =
                    taming;

                config.WeekendHarvestMultiplier =
                    harvest;

                config.WeekendBabyMatureMultiplier =
                    babyMature;

                config.WeekendXpMultiplier =
                    xp;

                config.WeekendWildDinoFoodDrainMultiplier =
                    wildDinoFoodDrain;

                config.WeekendBabyCuddleIntervalMultiplier =
                    babyCuddleInterval;

                if (WeekendStartDayBox.SelectedIndex < 0)
                {
                    StatusError(
                        "Jour de début invalide");

                    return;
                }


                if (WeekendEndDayBox.SelectedIndex < 0)
                {
                    StatusError(
                        "Jour de fin invalide");

                    return;
                }


                if (!int.TryParse(
                    WeekendStartHourBox.Text,
                    out int startHour) ||
                    startHour < 0 ||
                    startHour > 23)
                {
                    StatusError(
                        "Heure de début invalide");

                    return;
                }


                if (!int.TryParse(
                    WeekendStartMinuteBox.Text,
                    out int startMinute) ||
                    startMinute < 0 ||
                    startMinute > 59)
                {
                    StatusError(
                        "Minute de début invalide");

                    return;
                }


                if (!int.TryParse(
                    WeekendEndHourBox.Text,
                    out int endHour) ||
                    endHour < 0 ||
                    endHour > 23)
                {
                    StatusError(
                        "Heure de fin invalide");

                    return;
                }


                if (!int.TryParse(
                    WeekendEndMinuteBox.Text,
                    out int endMinute) ||
                    endMinute < 0 ||
                    endMinute > 59)
                {
                    StatusError(
                        "Minute de fin invalide");

                    return;
                }


                config.WeekendStartDay =
                    WeekendStartDayBox.SelectedIndex;

                config.WeekendStartHour =
                    startHour;

                config.WeekendStartMinute =
                    startMinute;


                config.WeekendEndDay =
                    WeekendEndDayBox.SelectedIndex;

                config.WeekendEndHour =
                    endHour;

                config.WeekendEndMinute =
                    endMinute;


                ConfigManager.Save(config);

                automation.ReloadConfig();


                WeekendPeriodText.Text =
                    $"Début : {GetDayName(config.WeekendStartDay)} " +
                    $"{config.WeekendStartHour:D2}:{config.WeekendStartMinute:D2}\n" +
                    $"Fin : {GetDayName(config.WeekendEndDay)} " +
                    $"{config.WeekendEndHour:D2}:{config.WeekendEndMinute:D2}";


                StatusOK(
                    "✔ Événement week-end sauvegardé");
            }
            catch (Exception ex)
            {
                StatusError(
                    ex.Message);
            }
        }


        // =========================
        // WEEKEND EVENT UI STATE
        // =========================

        private void WeekendEventBox_Changed(
            object sender,
            RoutedEventArgs e)
        {
            WeekendOptionsPanel.IsEnabled =
                WeekendEventBox.IsChecked == true;
        }


        // =========================
        // SET ORIGINAL CONFIG
        // =========================

        private async void SetOriginalConfig_Click(
            object sender,
            RoutedEventArgs e)

        {

            MessageBoxResult result =
    MessageBox.Show(
        "Voulez-vous vraiment définir la configuration actuelle du serveur comme configuration d'origine ?\n\n" +
        "L'ancienne configuration d'origine sera remplacée.",
        "Confirmer la configuration d'origine",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                SetOriginalConfigButton.IsEnabled = false;

                StatusText.Foreground =
                    Brushes.Yellow;

                StatusText.Text =
                    "🟡 Récupération de la configuration actuelle...";


                bool success =
                    await automation.SetCurrentConfigAsOriginalAsync();


                if (!success)
                {
                    StatusError(
                        "Impossible de définir la configuration d'origine");

                    return;
                }


                StatusOK(
                    "✅ Configuration actuelle définie comme origine");
            }
            catch (Exception ex)
            {
                StatusError(
                    ex.Message);
            }
            finally
            {
                SetOriginalConfigButton.IsEnabled = true;
            }
        }


        // =========================
        // GET DAY NAME
        // =========================

        private static string GetDayName(
    int day)
        {
            return day switch
            {
                0 => "Dimanche",
                1 => "Lundi",
                2 => "Mardi",
                3 => "Mercredi",
                4 => "Jeudi",
                5 => "Vendredi",
                6 => "Samedi",

                _ => "Inconnu"
            };
        }


        // =========================
        // STATUS
        // =========================

        private void StatusOK(
            string text)
        {
            StatusText.Foreground =
                Brushes.LightGreen;

            StatusText.Text =
                text;
        }


        private void StatusError(
            string text)
        {
            StatusText.Foreground =
                Brushes.OrangeRed;

            StatusText.Text =
                text;
        }
    }
}