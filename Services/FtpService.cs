using FluentFTP;
using ArkPilot.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

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

                client.Config.LogToConsole = true;
                client.Config.LogHost = true;

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
            IProgress<FtpProgress>? progress = null)
        {
            try
            {
                await DisconnectAsync();

                if (!await ConnectAsync())
                    return false;

                using var input =
                    await client!.OpenRead(
                        remotePath);

                string tempFile =
                    localPath + ".download";

                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }

                using var output =
                    File.Create(tempFile);

                var buffer =
                    new byte[81920];

                long totalRead = 0;

                int bytesRead;

                while ((bytesRead =
                    await input.ReadAsync(
                        buffer,
                        0,
                        buffer.Length)) > 0)
                {
                    await output.WriteAsync(
                        buffer,
                        0,
                        bytesRead);

                    totalRead +=
                        bytesRead;
                }

                await output.FlushAsync();

                output.Close();

                if (File.Exists(localPath))
                {
                    File.Delete(localPath);
                }

                File.Move(
                    tempFile,
                    localPath);


                LogService.Success(
                    $"Téléchargement terminé : {localPath}");

                return true;
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Téléchargement FTP impossible : {ex.Message}");

                return false;
            }
        }

        public async Task<bool> UploadFileAsync(
            string localPath,
            string remotePath)
        {
            try
            {
                if (!await ConnectAsync())
                    return false;


                var status =
                    await client!.UploadFile(
                        localPath,
                        remotePath,
                        FtpRemoteExists.Overwrite,
                        true,
                        FtpVerify.None);


                bool success =
                    status == FtpStatus.Success;


                if (success)
                {
                    LogService.Success(
                        $"Upload terminé : {remotePath}");
                }
                else
                {
                    LogService.Warning(
                        $"Upload incomplet : {remotePath}");
                }


                return success;
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Upload FTP impossible : {ex.Message}");

                return false;
            }
        }

        public void Dispose()
        {
            _ = DisconnectAsync();
        }
    }
}