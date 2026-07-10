using FluentFTP;
using ArkPilot.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ArkPilot.Services
{
    public class FtpService : IDisposable
    {
        private readonly string host;
        private readonly int port;
        private readonly string username;
        private readonly string password;

        private AsyncFtpClient? client;

        public bool IsConnected =>
            client != null && client.IsConnected;

        public FtpService(
            string host,
            int port,
            string username,
            string password)
        {
            this.host = host;
            this.port = port;
            this.username = username;
            this.password = password;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                if (IsConnected)
                    return true;

                client = new AsyncFtpClient(
                    host,
                    username,
                    password,
                    port);

                await client.AutoConnect();

                return client.IsConnected;
            }
            catch (Exception ex)
            {
                LogService.Error($"FTP connexion impossible : {ex.Message}");
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            if (client == null)
                return;

            try
            {
                await client.Disconnect();
            }
            catch
            {
            }

            client.Dispose();
            client = null;
        }

        public async Task<bool> TestConnectionAsync()
        {
            bool connected = await ConnectAsync();

            await DisconnectAsync();

            return connected;
        }

        public async Task<List<FtpItem>> GetDirectoryListingAsync(string path)
        {
            List<FtpItem> items = new();

            try
            {
                if (!await ConnectAsync())
                    return items;

                foreach (var item in await client!.GetListing(path))
                {
                    items.Add(
                        new FtpItem
                        {
                            Name = item.Name,
                            FullPath = item.FullName,
                            IsDirectory = item.Type == FtpObjectType.Directory,
                            Size = item.Size,
                            Modified = item.Modified
                        });
                }
            }
            catch (Exception ex)
            {
                LogService.Error($"FTP listing impossible : {ex.Message}");
            }

            return items;
        }

        public async Task<List<FtpItem>> GetFilesAsync(
    string remoteFolder)
        {
            var items =
                await GetDirectoryListingAsync(remoteFolder);

            return items
                .Where(item => !item.IsDirectory)
                .ToList();
        }

        public async Task<bool> DownloadFileAsync(
            string remotePath,
            string localPath,
            IProgress<FluentFTP.FtpProgress>? progress = null)
        {
            try
            {
                if (!await ConnectAsync())
                    return false;

                var status =
                    await client!.DownloadFile(
                        localPath,
                        remotePath,
                        FtpLocalExists.Overwrite,
                        FtpVerify.None,
                        progress);

                bool success =
                    status == FtpStatus.Success;

                if (success)
                    LogService.Success($"Téléchargement terminé : {localPath}");
                else
                    LogService.Warning($"Téléchargement incomplet : {remotePath}");

                return success;
            }
            catch (Exception ex)
            {
                LogService.Error($"Téléchargement FTP impossible : {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _ = DisconnectAsync();
        }
    }
}