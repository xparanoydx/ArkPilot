using ArkPilot.Config;
using ArkPilot.Managers;
using ArkPilot.Models;
using ArkPilot.Services;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;

namespace ArkPilot.Views
{
    public partial class FtpExplorerPage : Page
    {
        private readonly FtpExplorerManager manager;
        private readonly TransferManager transferManager;

        public FtpExplorerPage()
        {
            InitializeComponent();

            Header.Title = "📁 Explorateur FTP";
            Header.Subtitle = "Navigation dans les fichiers du serveur";
            Header.Status = "🟢 FTP";

            var config = ConfigManager.Load();

            var ftp =
                new FtpService(
                    config.FtpHost,
                    config.FtpPort,
                    config.FtpUser,
                    config.FtpPassword);

            manager = new FtpExplorerManager(ftp);
            transferManager = new TransferManager(ftp);

            Loaded += async (_, __) =>
            {
                await RefreshAsync();
            };

            BackButton.Click += async (_, __) =>
            {
                SetLoading(true);

                var items = await manager.GoBackAsync();

                FilesList.ItemsSource = items;
                UpdateStatistics(items);

                UpdatePath();
                SetLoading(false);
            };

            RefreshButton.Click += async (_, __) =>
            {
                await RefreshAsync();
            };

            OpenDownloadsButton.Click += (_, __) =>
            {
                string downloadsFolder =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.MyDocuments),
                        "ArkPilot",
                        "Downloads");

                Directory.CreateDirectory(
                    downloadsFolder);


                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = downloadsFolder,
                        UseShellExecute = true
                    });
            };

            DownloadButton.Click += async (_, __) =>
            {
                await DownloadSelectedAsync();
            };

            FilesList.MouseDoubleClick += async (_, __) =>
            {
                if (FilesList.SelectedItem is not FtpItem item)
                    return;

                if (!item.IsDirectory)
                    return;

                SetLoading(true);

                var items = await manager.OpenFolderAsync(item);

                FilesList.ItemsSource = items;
                UpdateStatistics(items);

                UpdatePath();
                SetLoading(false);
            };

        }

        private async System.Threading.Tasks.Task RefreshAsync()
        {
            SetLoading(true);

            var items = await manager.RefreshAsync();

            FilesList.ItemsSource = items;

            UpdateStatistics(items);

            UpdatePath();
            SetLoading(false);
        }

        private async System.Threading.Tasks.Task DownloadSelectedAsync()
        {
            if (FilesList.SelectedItem is not FtpItem item)
                return;

            if (item.IsDirectory)
            {
                MessageBox.Show(
                    "Le téléchargement de dossier n'est pas encore disponible.",
                    "Téléchargement",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            SetLoading(true);

            bool success =
                await transferManager.DownloadAsync(item.FullPath);

            SetLoading(false);

            MessageBox.Show(
                success
                    ? "Téléchargement terminé."
                    : "Téléchargement impossible.",
                "FTP",
                MessageBoxButton.OK,
                success ? MessageBoxImage.Information : MessageBoxImage.Error);
        }

        private void SetLoading(bool loading)
        {
            PathText.Text =
                loading
                    ? "Chargement FTP..."
                    : $"Chemin : {manager.CurrentPath}";

            BackButton.IsEnabled = !loading;
            RefreshButton.IsEnabled = !loading;
            DownloadButton.IsEnabled = !loading;
            FilesList.IsEnabled = !loading;
        }

        private void UpdateStatistics(IEnumerable<FtpItem> items)
        {
            int folders = 0;
            int files = 0;
            long totalSize = 0;

            foreach (var item in items)
            {
                if (item.IsDirectory)
                {
                    folders++;
                }
                else
                {
                    files++;
                    totalSize += item.Size;
                }
            }

            FoldersCard.Value = folders.ToString();
            FilesCard.Value = files.ToString();
            SizeCard.Value = FormatSize(totalSize);
        }

        private static string FormatSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };

            double size = bytes;

            int index = 0;

            while (size >= 1024 && index < units.Length - 1)
            {
                size /= 1024;
                index++;
            }

            return $"{size:0.##} {units[index]}";
        }

        private void UpdatePath()
        {
            PathText.Text =
                $"Chemin : {manager.CurrentPath}";
        }
    }
}