using ArkPilot.Models;
using ArkPilot.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkPilot.Managers
{
    public class FtpExplorerManager
    {
        private readonly FtpService ftp;

        public string CurrentPath { get; private set; } = "/";

        public FtpExplorerManager(FtpService ftp)
        {
            this.ftp = ftp;
        }

        public async Task<List<FtpItem>> RefreshAsync()
        {
            return await ftp.GetDirectoryListingAsync(CurrentPath);
        }

        public async Task<List<FtpItem>> OpenFolderAsync(FtpItem folder)
        {
            if (!folder.IsDirectory)
                return await RefreshAsync();

            CurrentPath = folder.FullPath;

            return await RefreshAsync();
        }

        public async Task<List<FtpItem>> GoBackAsync()
        {
            if (CurrentPath == "/")
                return await RefreshAsync();

            int index = CurrentPath.LastIndexOf('/');

            if (index <= 0)
                CurrentPath = "/";
            else
                CurrentPath = CurrentPath[..index];

            return await RefreshAsync();
        }
    }
}