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

            CleanButton.Click +=
                CleanButton_Click;

            FullBackupButton.Click += FullBackupButton_Click;

            ServerBackupButton.Click +=
                ServerBackupButton_Click;

            Header.Title = "💾 Backup Manager";
            Header.Subtitle = "Gestion des sauvegardes locales du serveur";
            Header.Status = "🟢 Prêt";

            var mainWindow =
                (MainWindow)Application.Current.MainWindow;

            var backupService =
                mainWindow.BackupService;

            var taskQueue =
                new TaskQueueService();

            var config =
                ConfigManager.Load();

            var nitradoService =
                new NitradoService(config);

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
                    zipService,
                    ftpService,
                    nitradoService);

            UpdateBackupInfo();
            RefreshHistory();
        }


        // =========================
        // CLEAN BUTTON
        // =========================


        private void CleanButton_Click(
    object? sender,
    EventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show(
                    "Supprimer les anciennes sauvegardes et conserver uniquement les 10 plus récentes ?",
                    "Nettoyage des sauvegardes",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
            {
                return;
            }


            int deletedCount =
                backupManager.CleanOldBackups();


            RefreshHistory();

            UpdateBackupInfo();


            MessageBox.Show(
                $"{deletedCount} sauvegarde(s) supprimée(s).",
                "Backup Manager",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
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

                    UpdateBackupInfo();

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


        private async void ServerBackupButton_Click(
    object? sender,
    EventArgs e)
        {
            try
            {
                ServerBackupButton.IsButtonEnabled =
                    false;


                bool success =
                    await backupManager.CreateServerBackupAsync();


                if (success)
                {
                    RefreshHistory();

                    UpdateBackupInfo();


                    MessageBox.Show(
                        "La sauvegarde complète du serveur est terminée.",
                        "Backup Manager",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Le téléchargement de la sauvegarde serveur a échoué.",
                        "Backup Manager",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Erreur sauvegarde serveur complète : {ex.Message}");


                MessageBox.Show(
                    ex.Message,
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                ServerBackupButton.IsButtonEnabled =
                    true;
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


            ArchiveCard.Value =
                backupManager
                    .GetBackupCount()
                    .ToString();


            SizeCard.Value =
                backupManager
                    .GetFormattedTotalBackupSize();


            FtpCard.Value =
                "Connecté";
        }
    }
}