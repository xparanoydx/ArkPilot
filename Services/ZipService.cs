using System;
using System.IO;
using System.IO.Compression;

namespace ArkPilot.Services
{
    public class ZipService
    {
        public bool CreateArchive(
            string sourceFolder,
            string destinationZip)
        {
            try
            {
                if (!Directory.Exists(sourceFolder))
                    return false;

                if (File.Exists(destinationZip))
                    File.Delete(destinationZip);

                ZipFile.CreateFromDirectory(
                    sourceFolder,
                    destinationZip,
                    CompressionLevel.Optimal,
                    false);

                LogService.Success(
                    $"Archive créée : {destinationZip}");

                return true;
            }
            catch (Exception ex)
            {
                LogService.Error(
                    $"Création du ZIP impossible : {ex.Message}");

                return false;
            }
        }

        public void DeleteFolder(string folder)
        {
            if (!Directory.Exists(folder))
                return;

            Directory.Delete(folder, true);

            LogService.Info(
                $"Dossier supprimé : {folder}");
        }
    }
}