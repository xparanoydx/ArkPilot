using ArkPilot.Services;
using System.Windows.Controls;
using ArkPilot.Config;
using System;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using ArkPilot.Models;

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

            automation.WeekendEventStateChanged +=
                Automation_WeekendEventStateChanged;

            automation.WeekendEventConfigApplied +=
                Automation_WeekendEventConfigApplied;


            config = ConfigManager.Load();

            LoadConfig();

            Unloaded +=
                EventsPage_Unloaded;
        }


        // =========================
        // PAGE UNLOADED
        // =========================

        private void EventsPage_Unloaded(
            object sender,
            RoutedEventArgs e)
        {
            automation.WeekendEventStateChanged -=
                Automation_WeekendEventStateChanged;

            automation.WeekendEventConfigApplied -=
                Automation_WeekendEventConfigApplied;
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

            LoadOriginalConfig();

            LoadActiveConfigState();

            LoadRestartRequiredState();

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
            if (automation.WeekendEventOperationInProgress)
            {
                StatusText.Foreground =
                    Brushes.Yellow;

                StatusText.Text =
                    "🟡 Application de la configuration déjà en cours...";

                return;
            }

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

                automation.CheckWeekendEventNow();


                WeekendPeriodText.Text =
                    $"Début : {GetDayName(config.WeekendStartDay)} " +
                    $"{config.WeekendStartHour:D2}:{config.WeekendStartMinute:D2}\n" +
                    $"Fin : {GetDayName(config.WeekendEndDay)} " +
                    $"{config.WeekendEndHour:D2}:{config.WeekendEndMinute:D2}";


                if (automation.WeekendEventActive == true)
                {
                    StatusText.Foreground =
                        Brushes.Yellow;

                    StatusText.Text =
                        "🟡 Modification sauvegardée — application en attente...";
                }
                else
                {
                    StatusOK(
                        "✔ Configuration week-end préparée");
                }
            }
            catch (Exception ex)
            {
                StatusError(
                    ex.Message);
            }
        }


        // =========================
        // LOAD ORIGINAL CONFIG
        // =========================

        private void LoadOriginalConfig()
        {
            OriginalEventConfig? original =
                automation.GetOriginalEventConfig();


            if (original == null)
            {
                OriginalTamingText.Text = "--";

                OriginalWildDinoFoodDrainText.Text = "--";

                OriginalHarvestText.Text = "--";

                OriginalBabyMatureText.Text = "--";

                OriginalBabyCuddleIntervalText.Text = "--";

                OriginalXpText.Text = "--";

                return;
            }


            OriginalTamingText.Text =
                original.TamingMultiplier.ToString(
                    CultureInfo.InvariantCulture);

            OriginalWildDinoFoodDrainText.Text =
                original.WildDinoFoodDrainMultiplier.ToString(
                    CultureInfo.InvariantCulture);

            OriginalHarvestText.Text =
                original.HarvestMultiplier.ToString(
                    CultureInfo.InvariantCulture);

            OriginalBabyMatureText.Text =
                original.BabyMatureMultiplier.ToString(
                    CultureInfo.InvariantCulture);

            OriginalBabyCuddleIntervalText.Text =
                original.BabyCuddleIntervalMultiplier.ToString(
                    CultureInfo.InvariantCulture);

            OriginalXpText.Text =
                original.XpMultiplier.ToString(
                    CultureInfo.InvariantCulture);
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

                LoadOriginalConfig();

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
                LoadActiveConfigState();
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
        // WEEKEND EVENT CONFIG APPLIED
        // =========================

        private void Automation_WeekendEventConfigApplied()
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Foreground =
                    Brushes.Orange;

                StatusText.Text =
                    "⚠ Configuration week-end mise à jour — redémarrage requis";
            });
        }


        // =========================
        // WEEKEND EVENT STATE CHANGED
        // =========================

        private void Automation_WeekendEventStateChanged()
        {
            Dispatcher.Invoke(() =>
            {
                LoadActiveConfigState();

                LoadRestartRequiredState();
            });
        }

        // =========================
        // LOAD ACTIVE CONFIG STATE
        // =========================

        private void LoadActiveConfigState()
        {
            bool? active =
                automation.WeekendEventActive;

            SetOriginalConfigButton.IsEnabled =
                active != true;

            OriginalConfigWarningText.Visibility =
                active == true
                ? Visibility.Visible
                : Visibility.Collapsed;

            if (active == true)
            {
                WeekendConfigStateText.Text =
                    "CONFIGURATION ACTIVE";

                WeekendConfigStateText.Foreground =
                    Brushes.LightGreen;
            }
            else if (active == false)
            {
                WeekendConfigStateText.Text =
                    "CONFIGURATION EN ATTENTE";

                WeekendConfigStateText.Foreground =
                    Brushes.Gray;
            }
            else
            {
                WeekendConfigStateText.Text =
                    "ÉTAT INCONNU";

                WeekendConfigStateText.Foreground =
                    Brushes.Orange;
            }

            if (active == true)
            {
                OriginalConfigStateText.Text =
                    "CONFIGURATION EN ATTENTE";

                OriginalConfigStateText.Foreground =
                    Brushes.Gray;
            }
            else if (active == false)
            {
                OriginalConfigStateText.Text =
                    "CONFIGURATION ACTIVE";

                OriginalConfigStateText.Foreground =
                    Brushes.LightSkyBlue;
            }
            else
            {
                OriginalConfigStateText.Text =
                    "ÉTAT INCONNU";

                OriginalConfigStateText.Foreground =
                    Brushes.Orange;
            }


            WeekendArrowText.Foreground =
                Brushes.Gray;

            WeekArrowText.Foreground =
                Brushes.Gray;

            WeekendTitleText.Foreground =
                Brushes.Gray;

            WeekTitleText.Foreground =
                Brushes.Gray;


            if (active == true)
            {
                ActiveConfigText.Text =
                    "🎉 WEEK-END ACTIF";

                ActiveConfigText.Foreground =
                    Brushes.LightGreen;

                WeekendArrowText.Foreground =
                    Brushes.LightGreen;

                WeekendTitleText.Foreground =
                    Brushes.LightGreen;

                return;
            }


            if (active == false)
            {
                ActiveConfigText.Text =
                    "📅 SEMAINE ACTIVE";

                ActiveConfigText.Foreground =
                    Brushes.LightSkyBlue;

                WeekArrowText.Foreground =
                    Brushes.LightSkyBlue;

                WeekTitleText.Foreground =
                    Brushes.LightSkyBlue;

                return;
            }


            ActiveConfigText.Text =
                "ÉTAT INCONNU";

            ActiveConfigText.Foreground =
                Brushes.Orange;
        }


        // =========================
        // LOAD RESTART REQUIRED STATE
        // =========================

        private void LoadRestartRequiredState()
        {
            RestartRequiredBorder.Visibility =
                automation.WeekendRestartRequired
                    ? Visibility.Visible
                    : Visibility.Collapsed;
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