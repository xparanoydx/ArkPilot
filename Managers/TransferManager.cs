using FluentFTP;
using ArkPilot.Services;
using ArkPilot.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace ArkPilot.Managers
{
    public class TransferManager
    {
        private readonly FtpService ftp;

        public TransferManager(FtpService ftp)
        {
            this.ftp = ftp;
        }

        public async Task<bool> TestConnectionAsync()
        {
            return await ftp.TestConnectionAsync();
        }

        // =========================
        // TÉLÉCHARGEMENT STANDARD
        // =========================

        public async Task<bool> DownloadAsync(
            string remoteFile)
        {
            string folder =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments),
                    "ArkPilot",
                    "Downloads");

            Directory.CreateDirectory(folder);

            string localFile =
                Path.Combine(
                    folder,
                    Path.GetFileName(remoteFile));

            var window = CreateProgressWindow(
                Path.GetFileName(remoteFile));

            try
            {
                var progress =
                    new Progress<FtpProgress>(p =>
                    {
                        window.SetProgress(p.Progress);
                    });

                bool result =
                    await ftp.DownloadFileAsync(
                        remoteFile,
                        localFile,
                        progress);

                if (result)
                {
                    window.SetProgress(100);
                    await Task.Delay(500);
                }

                return result;
            }
            finally
            {
                window.Close();
            }
        }

        // =========================
        // SAUVEGARDE DU MONDE SEULE
        // =========================

        public async Task<bool> DownloadWorldBackupAsync(
            string remoteFile)
        {
            string folder =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments),
                    "ArkPilot",
                    "Backups");

            Directory.CreateDirectory(folder);

            string localFile =
                Path.Combine(
                    folder,
                    Path.GetFileName(remoteFile));

            var window = CreateProgressWindow(
                Path.GetFileName(remoteFile));

            try
            {
                var progress =
                    new Progress<FtpProgress>(p =>
                    {
                        window.SetProgress(p.Progress);
                    });

                bool result =
                    await ftp.DownloadFileAsync(
                        remoteFile,
                        localFile,
                        progress);

                if (result)
                {
                    window.SetProgress(100);
                    await Task.Delay(500);
                }

                return result;
            }
            finally
            {
                window.Close();
            }
        }

        // =========================
        // TÉLÉCHARGEMENT D'UN DOSSIER
        // =========================

        public async Task<bool> DownloadFolderAsync(
            string remoteFolder,
            string localFolder)
        {
            Directory.CreateDirectory(localFolder);

            var files =
                await ftp.GetFilesAsync(remoteFolder);

            if (files.Count == 0)
            {
                LogService.Warning(
                    $"Aucun fichier trouvé dans : {remoteFolder}");

                return false;
            }

            var window = CreateProgressWindow(
                "Préparation du téléchargement...");

            try
            {
                for (int index = 0;
                     index < files.Count;
                     index++)
                {
                    var file = files[index];

                    window.SetFile(
                        $"{index + 1} / {files.Count} — {file.Name}");

                    string localFile =
                        Path.Combine(
                            localFolder,
                            file.Name);

                    int currentIndex = index;

                    var progress =
                        new Progress<FtpProgress>(p =>
                        {
                            double globalProgress =
                                ((currentIndex +
                                  (p.Progress / 100.0))
                                 / files.Count)
                                * 100.0;

                            window.SetProgress(globalProgress);
                        });

                    bool success =
                        await ftp.DownloadFileAsync(
                            file.FullPath,
                            localFile,
                            progress);

                    if (!success)
                    {
                        LogService.Error(
                            $"Échec du téléchargement : {file.Name}");

                        return false;
                    }
                }

                window.SetProgress(100);

                await Task.Delay(500);

                LogService.Success(
                    $"{files.Count} fichier(s) téléchargé(s)");

                return true;
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Téléchargement du dossier impossible : {ex.Message}");

                return false;
            }
            finally
            {
                window.Close();
            }
        }

        // =========================
        // FENÊTRE DE PROGRESSION
        // =========================

        private static TransferProgressWindow CreateProgressWindow(
            string fileName)
        {
            var window =
                new TransferProgressWindow
                {
                    Owner =
                        Application.Current.MainWindow
                };

            window.SetFile(fileName);
            window.SetProgress(0);
            window.Show();

            return window;
        }
    }
}