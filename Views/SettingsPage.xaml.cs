using ArkPilot.Config;
using ArkPilot.Services;
using System.Diagnostics;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArkPilot.Views
{
    public partial class SettingsPage : Page
    {
        private ServerConfig config;
        private readonly RconEngine rcon;

        private readonly AutomationService automation;

        private bool passwordVisible = false;

        public SettingsPage(
            RconEngine engine,
            AutomationService automationService)
        {
            InitializeComponent();

            rcon = engine;

            automation = automationService;

            config = ConfigManager.Load();

            LoadConfig();
        }

        // =========================
        // CHARGEMENT CONFIG
        // =========================

        private void LoadConfig()
        {
            ServerNameBox.Text = config.ServerName;
            IpBox.Text = config.ServerIp;
            PortBox.Text = config.RconPort.ToString();

            PasswordBox.Password = config.RconPassword;
            PasswordTextBox.Text = config.RconPassword;

            ApiKeyBox.Text = config.NitradoApiKey;
            ServiceIdBox.Text = config.NitradoServiceId;

            AutoConnectBox.IsChecked = config.AutoConnect;

            AutoSaveBox.IsChecked =
    config.AutoSaveEnabled;

            AutoSaveIntervalBox.Text =
                config.AutoSaveIntervalMinutes.ToString();

            AutoRestartBox.IsChecked =
                config.AutoRestartEnabled;

            AutoRestartHourBox.Text =
                config.AutoRestartHour.ToString();

            AutoRestartMinuteBox.Text =
                config.AutoRestartMinute.ToString();

            RefreshBox.Text =
                config.RefreshInterval.ToString();

            StatusText.Text =
                "⚪ Configuration chargée";

            FtpHostBox.Text = config.FtpHost;
            FtpPortBox.Text = config.FtpPort.ToString();
            FtpUserBox.Text = config.FtpUser;
            FtpPasswordBox.Password = config.FtpPassword;
        }


        // =========================
        // SAUVEGARDE
        // =========================

        private async void Save_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                config.ServerName =
                    ServerNameBox.Text.Trim();

                config.ServerIp =
                    IpBox.Text.Trim();

                config.RconPassword =
                    passwordVisible
                        ? PasswordTextBox.Text
                        : PasswordBox.Password;

                config.NitradoApiKey =
                    ApiKeyBox.Text.Trim();

                config.NitradoServiceId =
                    ServiceIdBox.Text.Trim();

                if (!int.TryParse(
                        PortBox.Text,
                        out int port))
                {
                    StatusError("Port RCON invalide");
                    return;
                }

                if (!int.TryParse(
                        RefreshBox.Text,
                        out int refresh))
                {
                    StatusError("Refresh invalide");
                    return;
                }

                config.RconPort = port;
                config.RefreshInterval = refresh;

                config.AutoConnect =
                    AutoConnectBox.IsChecked == true;

                config.AutoSaveEnabled =
    AutoSaveBox.IsChecked == true;


                if (!int.TryParse(
                    AutoSaveIntervalBox.Text,
                    out int autoSaveInterval))
                {
                    StatusError(
                        "Intervalle de sauvegarde invalide");

                    return;
                }


                config.AutoSaveIntervalMinutes =
                    autoSaveInterval;

                if (autoSaveInterval < 1)
                {
                    StatusError(
                        "L'intervalle de sauvegarde doit être supérieur à 0");

                    return;
                }


                config.AutoRestartEnabled =
                    AutoRestartBox.IsChecked == true;


                if (!int.TryParse(
                    AutoRestartHourBox.Text,
                    out int autoRestartHour))
                {
                    StatusError(
                        "Heure de redémarrage invalide");

                    return;
                }


                if (!int.TryParse(
                    AutoRestartMinuteBox.Text,
                    out int autoRestartMinute))
                {
                    StatusError(
                        "Minute de redémarrage invalide");

                    return;
                }


                if (autoRestartHour < 0 ||
                    autoRestartHour > 23)
                {
                    StatusError(
                        "L'heure de redémarrage doit être comprise entre 0 et 23");

                    return;
                }


                if (autoRestartMinute < 0 ||
                    autoRestartMinute > 59)
                {
                    StatusError(
                        "La minute de redémarrage doit être comprise entre 0 et 59");

                    return;
                }


                config.AutoRestartHour =
                    autoRestartHour;

                config.AutoRestartMinute =
                    autoRestartMinute;

                config.FtpHost = FtpHostBox.Text.Trim();
                config.FtpUser = FtpUserBox.Text.Trim();
                config.FtpPassword = FtpPasswordBox.Password;

                config.FtpUser =
                    FtpUserBox.Text.Trim();

                config.FtpPassword =
                    FtpPasswordBox.Password;

                if (!int.TryParse(
                        FtpPortBox.Text,
                        out int ftpPort))
                {
                    StatusError("Port FTP invalide");
                    return;
                }

                ConfigManager.Save(config);

                automation.ReloadConfig();


                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    bool connected =
                        await mainWindow.InitializeServerAsync();


                    if (connected)
                    {
                        StatusOK(
                            "✔ Configuration sauvegardée et serveur connecté");
                    }
                    else
                    {
                        StatusError(
                            "Configuration sauvegardée mais connexion RCON impossible");
                    }


                    return;
                }


                StatusOK(
                    "✔ Configuration sauvegardée");

            }
            catch (Exception ex)
            {
                StatusError(
                    "Erreur sauvegarde : " + ex.Message);
            }
        }


        // =========================
        // TEST RCON
        // =========================

        private async void TestRcon_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                StatusText.Foreground =
                    System.Windows.Media.Brushes.Yellow;

                StatusText.Text =
                    "🟡 Connexion RCON...";


                Stopwatch timer = new Stopwatch();

                timer.Start();


                bool connected =
                    await rcon.Connect(
                        IpBox.Text.Trim(),
                        int.Parse(PortBox.Text),
                        PasswordBox.Password);


                timer.Stop();


                if (connected)
                {
                    Stopwatch pingTimer = new Stopwatch();

                    pingTimer.Start();


                    await rcon.Query("ListPlayers");

                    pingTimer.Stop();


                    StatusText.Foreground =
                        System.Windows.Media.Brushes.LightGreen;


                    StatusText.Text =
                        $"🟢 RCON connecté | Ping {pingTimer.ElapsedMilliseconds} ms";
                }
                else
                {
                    StatusText.Foreground =
                        System.Windows.Media.Brushes.OrangeRed;

                    StatusText.Text =
                        "🔴 Connexion RCON refusée";
                }
            }
            catch (Exception ex)
            {
                StatusText.Foreground =
                    System.Windows.Media.Brushes.OrangeRed;

                StatusText.Text =
                    "Erreur RCON : " + ex.Message;
            }
        }



        // =========================
        // TEST API NITRADO
        // =========================

        private async void TestApi_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                StatusText.Foreground = Brushes.Yellow;
                StatusText.Text = "🟡 Test API Nitrado...";

                var testConfig = new ServerConfig
                {
                    NitradoApiKey = ApiKeyBox.Text.Trim(),
                    NitradoServiceId = ServiceIdBox.Text.Trim()
                };

                var nitrado =
                    new NitradoService(testConfig);

                string result =
                    await nitrado.GetStatusAsync();

                if (result == "NITRADO_NOT_CONFIGURED")
                {
                    StatusError("API Nitrado non configurée");
                    return;
                }

                if (result == "NITRADO_ERROR")
                {
                    StatusError("Erreur API Nitrado");
                    return;
                }

                var info = await nitrado.GetServerInfoAsync();

                StatusText.Foreground = Brushes.LightGreen;

                StatusText.Text =
                    $"🟢 API connectée\n" +
                    $"État : {info.Status}\n" +
                    $"Carte : {info.Map}\n" +
                    $"Joueurs : {info.SlotsUsed} / {info.SlotsMax}\n" +
                    $"Version : {info.Version}";
            }
            catch (Exception ex)
            {
                StatusError("Erreur API : " + ex.Message);
            }
        }



        // =========================
        // TEST FTP NITRADO
        // =========================


        private async void TestFtp_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                StatusText.Foreground = Brushes.Yellow;
                StatusText.Text = "🟡 Test connexion FTP...";

                if (!int.TryParse(FtpPortBox.Text, out int ftpPort))
                {
                    StatusError("Port FTP invalide");
                    return;
                }

                var ftp =
                    new FtpService(
                        FtpHostBox.Text.Trim(),
                        ftpPort,
                        FtpUserBox.Text.Trim(),
                        FtpPasswordBox.Password);

                bool connected =
                    await ftp.TestConnectionAsync();

                if (connected)
                {
                    StatusOK("🟢 FTP connecté");
                }
                else
                {
                    StatusError("🔴 Connexion FTP impossible");
                }
            }
            catch (Exception ex)
            {
                StatusError("Erreur FTP : " + ex.Message);
            }
        }

        // =========================
        // RELOAD
        // =========================

        private void Reload_Click(
            object sender,
            RoutedEventArgs e)
        {
            config =
                ConfigManager.Load();

            LoadConfig();

            StatusOK(
                "🔄 Configuration rechargée");
        }



        // =========================
        // PASSWORD
        // =========================

        private void ShowPassword_Click(
            object sender,
            RoutedEventArgs e)
        {
            passwordVisible = !passwordVisible;


            if (passwordVisible)
            {
                PasswordTextBox.Text =
                    PasswordBox.Password;

                PasswordBox.Visibility =
                    Visibility.Collapsed;

                PasswordTextBox.Visibility =
                    Visibility.Visible;

                ShowPasswordButton.Content = "🔒";
            }
            else
            {
                PasswordBox.Password =
                    PasswordTextBox.Text;

                PasswordBox.Visibility =
                    Visibility.Visible;

                PasswordTextBox.Visibility =
                    Visibility.Collapsed;

                ShowPasswordButton.Content = "👁";
            }
        }


        // =========================
        // STATUS
        // =========================

        private void StatusOK(string text)
        {
            StatusText.Foreground =
                Brushes.LightGreen;

            StatusText.Text = text;
        }


        private void StatusError(string text)
        {
            StatusText.Foreground =
                Brushes.OrangeRed;

            StatusText.Text = text;
        }
    }
}