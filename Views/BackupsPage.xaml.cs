using ArkPilot.Core;
using ArkPilot.Config;
using ArkPilot.Managers;
using ArkPilot.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Views
{
    public partial class BackupsPage : Page
    {
        private readonly BackupManager backupManager;

        public BackupsPage()
        {
            InitializeComponent();

            QuickBackupButton.Click += QuickBackupButton_Click;

            OpenFolderButton.Click += OpenFolderButton_Click;

            FullBackupButton.Click += FullBackupButton_Click;

            Header.Title = "💾 Backup Manager";
            Header.Subtitle = "Gestion des sauvegardes locales du serveur";
            Header.Status = "🟢 Prêt";

            var mainWindow =
                (MainWindow)Application.Current.MainWindow;

            var ark =
                new ArkService(mainWindow.Rcon);

            var backupService =
                new BackupService(ark);

            var taskQueue =
                new TaskQueueService();

            var config =
    ConfigManager.Load();

            var ftpService =
                new FtpService(
                    config.FtpHost,
                    config.FtpPort,
                    config.FtpUser,
                    config.FtpPassword);

            var transferManager =
                new TransferManager(ftpService);

            var zipService =
                new ZipService();

            backupManager =
                new BackupManager(
                    backupService,
                    taskQueue,
                    transferManager,
                    zipService);

            UpdateBackupInfo();
            RefreshHistory();
        }

        private async void FullBackupButton_Click(
    object? sender,
    EventArgs e)
        {
            try
            {
                FullBackupButton.IsButtonEnabled = false;

                bool success =
                    await backupManager.CreateWorldBackupAsync();

                if (success)
                {
                    RefreshHistory();

                    MessageBox.Show(
                        "La sauvegarde du monde est terminée.",
                        "Backup Manager",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Le téléchargement de la sauvegarde a échoué.",
                        "Backup Manager",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Erreur sauvegarde complète : {ex.Message}");

                MessageBox.Show(
                    ex.Message,
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                FullBackupButton.IsButtonEnabled = true;
            }
        }


        private void OpenFolderButton_Click(
    object? sender,
    EventArgs e)
        {
            try
            {
                backupManager.OpenBackupFolder();
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Impossible d'ouvrir le dossier : {ex.Message}");

                MessageBox.Show(
                    ex.Message,
                    "Backup Manager",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void QuickBackupButton_Click(
    object? sender,
    EventArgs e)
        {
            try
            {
                QuickBackupButton.IsButtonEnabled = false;

                await backupManager.SaveNowAsync();

                UpdateBackupInfo();

                MessageBox.Show(
                    "La sauvegarde a été demandée au serveur.",
                    "Backup Manager",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LogService.Error($"Erreur sauvegarde : {ex.Message}");

                MessageBox.Show(
                    ex.Message,
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                QuickBackupButton.IsButtonEnabled = true;
            }
        }

        private void RefreshHistory()
        {
            HistoryList.ItemsSource =
                backupManager.GetBackups();
        }

        private void UpdateBackupInfo()
        {
            LastBackupCard.Value =
                backupManager.LastBackupTime == default
                    ? "--"
                    : backupManager.LastBackupTime.ToString("HH:mm:ss");

            ArchiveCard.Value = "0";

            SizeCard.Value = "0 MB";

            FtpCard.Value =
                "Connecté";
        }
    }
}