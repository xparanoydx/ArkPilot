using ArkPilot.Core;
using ArkPilot.Models;
using ArkPilot.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArkPilot.Managers
{
    public class BackupManager
    {

        private readonly BackupService backupService;
        private readonly TaskQueueService taskQueue;
        private readonly TransferManager transferManager;
        private readonly ZipService zipService;
        private readonly FtpService ftpService;
        private readonly NitradoService nitradoService;

        public DateTime LastBackupTime =>
            backupService.LastBackupTime;

        public BackupManager(
            BackupService backupService,
            TaskQueueService taskQueue,
            TransferManager transferManager,
            ZipService zipService,
            FtpService ftpService,
            NitradoService nitradoService)
        {
            this.backupService = backupService;
            this.taskQueue = taskQueue;
            this.transferManager = transferManager;
            this.zipService = zipService;
            this.ftpService = ftpService;
            this.nitradoService = nitradoService;
        }

        // =========================
        // DOSSIER PRINCIPAL
        // =========================

        private static string GetBackupRootFolder()
        {
            string folder =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments),
                    "ArkPilot",
                    "Backups");

            Directory.CreateDirectory(folder);

            return folder;
        }

        // =========================
        // SAVEWORLD SIMPLE
        // =========================

        public Task SaveNowAsync()
        {
            return taskQueue.Enqueue(
                "SaveWorld manuel",
                () =>
                {
                    backupService.SaveNow();

                    return Task.CompletedTask;
                });
        }

        private async Task<string?> FindWorldFolderAsync()
        {
            const string savedArksPath =
                "/arksa/ShooterGame/Saved/SavedArks/";

            var serverInfo =
                await nitradoService.GetServerInfoAsync();

            if (string.IsNullOrWhiteSpace(serverInfo.Map) ||
                serverInfo.Map == "--")
            {
                LogService.Error(
                    "Impossible de déterminer la map active.");

                return null;
            }

            string activeMap =
                serverInfo.Map.Trim();

            LogService.Info(
                $"Map active détectée : {activeMap}");

            var items =
                await ftpService.GetDirectoryListingAsync(
                    savedArksPath);

            var worldFolder =
                items.FirstOrDefault(item =>
                    item.IsDirectory &&
                    item.Name.Equals(
                        activeMap,
                        StringComparison.OrdinalIgnoreCase));

            if (worldFolder == null)
            {
                LogService.Error(
                    $"Dossier FTP de la map introuvable : {activeMap}");

                return null;
            }

            LogService.Success(
                $"Dossier monde sélectionné : {worldFolder.Name}");

            return worldFolder.FullPath + "/";
        }


        // =========================
        // SAUVEGARDE COMPLÈTE
        // =========================

        public async Task<bool> CreateWorldBackupAsync()
        {
            bool success = false;

            await taskQueue.Enqueue(
                "Sauvegarde complète du monde ARK",
                async () =>
                {
                    backupService.SaveNow();

                    LogService.Info(
                        "Attente de la fin du SaveWorld...");

                    await Task.Delay(
                        TimeSpan.FromSeconds(10));

                    string backupName =
                        $"Backup_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}";

                    string backupRoot =
                        GetBackupRootFolder();

                    string temporaryFolder =
                        Path.Combine(
                            backupRoot,
                            backupName);

                    string worldFolder =
                        Path.Combine(
                            temporaryFolder,
                            "World");

                    string zipFile =
                        Path.Combine(
                            backupRoot,
                            backupName + ".zip");

                    Directory.CreateDirectory(worldFolder);

                    string? worldFolderRemotePath =
    await FindWorldFolderAsync();

                    if (worldFolderRemotePath == null)
                    {
                        success = false;
                        return;
                    }

                    bool downloadSuccess =
                        await transferManager.DownloadFolderAsync(
                            worldFolderRemotePath,
                            worldFolder);

                    if (!downloadSuccess)
                    {
                        LogService.Error(
                            "Le téléchargement du monde a échoué.");

                        success = false;
                        return;
                    }

                    LogService.Info(
                        "Création de l'archive ZIP...");

                    bool zipSuccess =
                        zipService.CreateArchive(
                            temporaryFolder,
                            zipFile);

                    if (!zipSuccess)
                    {
                        LogService.Error(
                            "La compression ZIP a échoué.");

                        success = false;
                        return;
                    }

                    try
                    {
                        zipService.DeleteFolder(
                            temporaryFolder);
                    }
                    catch (Exception ex)
                    {
                        LogService.Warning(
                            $"Archive créée, mais le dossier temporaire n'a pas pu être supprimé : {ex.Message}");
                    }

                    success = true;

                    LogService.Success(
                        $"Sauvegarde complète créée : {zipFile}");
                });

            return success;
        }

        // =========================
        // HISTORIQUE
        // =========================

        public List<BackupFile> GetBackups()
        {
            string folder =
                GetBackupRootFolder();

            var backups =
                new List<(BackupFile Backup, DateTime Modified)>();

            // Dossiers temporaires ou sauvegardes incomplètes
            foreach (string directory in
                     Directory.GetDirectories(folder))
            {
                var info =
                    new DirectoryInfo(directory);

                backups.Add(
                    (
                        new BackupFile
                        {
                            FileName = "📁 " + info.Name,
                            Date = info.LastWriteTime
                                .ToString("dd/MM/yyyy HH:mm"),
                            Size = FormatSize(
                                GetDirectorySize(directory))
                        },
                        info.LastWriteTime
                    ));
            }

            // Archives ZIP et anciens fichiers .ark
            foreach (string file in
                     Directory.GetFiles(folder))
            {
                if (!file.EndsWith(
                        ".zip",
                        StringComparison.OrdinalIgnoreCase) &&
                    !file.EndsWith(
                        ".ark",
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var info =
                    new FileInfo(file);

                backups.Add(
                    (
                        new BackupFile
                        {
                            FileName =
                                file.EndsWith(
                                    ".zip",
                                    StringComparison.OrdinalIgnoreCase)
                                    ? "📦 " + info.Name
                                    : "🦖 " + info.Name,

                            Date = info.LastWriteTime
                                .ToString("dd/MM/yyyy HH:mm"),

                            Size = FormatSize(info.Length)
                        },
                        info.LastWriteTime
                    ));
            }

            return backups
                .OrderByDescending(item => item.Modified)
                .Select(item => item.Backup)
                .ToList();
        }

        private static long GetDirectorySize(
            string directory)
        {
            try
            {
                return Directory
                    .EnumerateFiles(
                        directory,
                        "*",
                        SearchOption.AllDirectories)
                    .Sum(file =>
                    {
                        try
                        {
                            return new FileInfo(file).Length;
                        }
                        catch
                        {
                            return 0L;
                        }
                    });
            }
            catch
            {
                return 0;
            }
        }

        private static string FormatSize(
            long bytes)
        {
            string[] units =
            {
                "B",
                "KB",
                "MB",
                "GB",
                "TB"
            };

            double size = bytes;
            int index = 0;

            while (size >= 1024 &&
                   index < units.Length - 1)
            {
                size /= 1024;
                index++;
            }

            return $"{size:0.##} {units[index]}";
        }

        // =========================
        // OUVERTURE DU DOSSIER
        // =========================

        public void OpenBackupFolder()
        {
            string folder =
                GetBackupRootFolder();

            Process.Start(
                new ProcessStartInfo
                {
                    FileName = folder,
                    UseShellExecute = true
                });
        }
    }
}