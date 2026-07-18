using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using ArkPilot.Models;
using AsaSavegameToolkit.Porcelain;
using AsaSavegameToolkit;
using ArkPilot.Helpers;

namespace ArkPilot.Services
{
    public class ArkSaveDataService
    {
        private readonly FtpService ftpService;

        private readonly NitradoService nitradoService;

        private AsaSaveGame? _cachedSave;

        private string? _cachedWorldFile;

        private DateTime _cachedLoadTimeUtc;

        private long _cachedWorldFileSize;

        private DateTime _cachedWorldFileWriteTimeUtc;



        public ArkSaveDataService(
            FtpService ftpService,
            NitradoService nitradoService)
        {
            this.ftpService =
                ftpService;

            this.nitradoService =
                nitradoService;
        }


        private async Task<AsaSaveGame?> GetSaveAsync()
        {
            string? worldFile =
                await DownloadWorldFileAsync();

            if (string.IsNullOrWhiteSpace(worldFile) ||
                !File.Exists(worldFile))
            {
                return null;
            }

            var fileInfo =
                new FileInfo(worldFile);

            bool cachedFileIsCurrent =
                _cachedSave != null &&
                string.Equals(
                    _cachedWorldFile,
                    worldFile,
                    StringComparison.OrdinalIgnoreCase) &&
                _cachedWorldFileSize == fileInfo.Length &&
                _cachedWorldFileWriteTimeUtc == fileInfo.LastWriteTimeUtc;

            if (cachedFileIsCurrent)
            {
                LogService.Info(
                    "SAVE DATA | Sauvegarde déjà chargée en mémoire.");

                return _cachedSave;
            }

            var settings =
                new AsaReaderSettings
                {
                    ReadGameObjects = true,
                    ReadArkProfileFiles = true,
                    ReadArkTribeFiles = true,
                    ReadCryoObjects = false
                };

            LogService.Info(
                "SAVE DATA | Lecture de la sauvegarde...");

            _cachedSave =
                AsaSaveGame.ReadFrom(
                    worldFile,
                    settings: settings);

            _cachedWorldFile =
                worldFile;

            _cachedWorldFileSize =
                fileInfo.Length;

            _cachedWorldFileWriteTimeUtc =
                fileInfo.LastWriteTimeUtc;

            _cachedLoadTimeUtc =
                DateTime.UtcNow;

            LogService.Info(
                "SAVE DATA | Sauvegarde chargée en mémoire.");

            return _cachedSave;
        }


        public async Task<string?> FindWorldFolderAsync()
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


            return worldFolder.FullPath + "/";
        }

        public async Task<FtpItem?> FindWorldFileAsync()
        {
            string? worldFolder =
                await FindWorldFolderAsync();


            if (string.IsNullOrWhiteSpace(worldFolder))
            {
                return null;
            }


            var files =
                await ftpService.GetFilesAsync(
                    worldFolder);


            var worldFile =
                files.FirstOrDefault(file =>
                    file.Name.EndsWith(
                        ".ark",
                        StringComparison.OrdinalIgnoreCase));

            LogService.Info(
    $"FTP | {worldFile?.Name} | " +
    $"Date={worldFile?.Modified:dd/MM/yyyy HH:mm:ss} | " +
    $"Taille={worldFile?.Size:N0}");


            if (worldFile == null)
            {
                LogService.Error(
                    "Fichier monde .ark introuvable.");

                return null;
            }


            return worldFile;
        }


        public async Task<string?> DownloadWorldFileAsync()
        {
            var worldFile =
                await FindWorldFileAsync();


            if (worldFile == null)
            {
                return null;
            }


            string localFolder =
                Path.Combine(
                    Path.GetTempPath(),
                    "ArkPilot",
                    "SaveData");


            Directory.CreateDirectory(
                localFolder);


            string localFile =
                Path.Combine(
                    localFolder,
                    worldFile.Name);


            bool remoteFileUnchanged = false;

            if (File.Exists(localFile))
            {
                var localInfo =
                    new FileInfo(localFile);

                remoteFileUnchanged =
                    localInfo.Length == worldFile.Size &&
                    localInfo.LastWriteTimeUtc == 
                    worldFile.Modified.ToUniversalTime();
            }

            if (remoteFileUnchanged)
            {
                LogService.Info(
                    "FTP | Le fichier monde est déjà à jour.");

                return localFile;
            }


            bool success =
                await ftpService.DownloadFileAsync(
                    worldFile.FullPath,
                    localFile);


            if (!success)
            {
                LogService.Error(
                    "Téléchargement du fichier monde impossible.");

                return null;
            }


            File.SetLastWriteTimeUtc(
                localFile,
                worldFile.Modified.ToUniversalTime());



            var fileInfo =
                new FileInfo(localFile);


            return localFile;
        }


        public void InvalidateCache()
        {
            _cachedSave = null;
            _cachedWorldFile = null;
            _cachedWorldFileSize = 0;
            _cachedWorldFileWriteTimeUtc = DateTime.MinValue;
            _cachedLoadTimeUtc = DateTime.MinValue;

            LogService.Info(
                "SAVE DATA | Cache mémoire invalidé.");
        }



        public async Task<ArkSaveDataResult> LoadTribesAsync()
        {
            var save =
                await GetSaveAsync();

            if (save == null)
            {
                return new ArkSaveDataResult
                {
                    Success = false,
                    Error = "Fichier monde indisponible."
                };
            }


            LogService.Info(
                $"SAVE DATA | Joueurs détectés par le parseur : {save.Players.Count}");

            var playersByDataId =
                save.Players.Values
                    .Where(player =>
                        player.PlayerDataId.HasValue)
                    .GroupBy(player =>
                        player.PlayerDataId!.Value.ToString())
                    .ToDictionary(
                        group => group.Key,
                        group => group.First());


            var tribes =
                save.Tribes.Values
                    .Where(tribe => tribe != null)
                    .Select(tribe =>
                        new TribeInfo
                        {
                            Name =
                                string.IsNullOrWhiteSpace(tribe.TribeName)
                                    ? $"Tribu {tribe.TribeId}"
                                    : tribe.TribeName,

                            Id =
                                tribe.TribeId.ToString(),

                            MemberCount =
                                tribe.MemberNames.Length,

                            Members =
                                tribe.MemberNames
                                    .Where(member =>
                                        !string.IsNullOrWhiteSpace(member))
                                    .ToList(),

                            MemberDetails =
                                tribe.MemberIds
                                    .Select((memberId, index) =>
                                    {
                                        playersByDataId.TryGetValue(
                                            memberId,
                                            out var player);


                                        return new TribeMemberInfo
                                        {
                                            CharacterName =
                                                player?.CharacterName
                                                ?? (index < tribe.MemberNames.Length
                                                    ? tribe.MemberNames[index]
                                                    : ""),

                                            PlayerName =
                                                player?.PlayerName
                                                ?? "",

                                            PlayerDataId =
                                                memberId,

                                            Level =
                                                player == null
                                                    ? 0
                                                    : (int)player.Level,

                                            IsOwner =
                                                tribe.OwnerPlayerDataId.HasValue &&
                                                tribe.OwnerPlayerDataId.Value.ToString() == memberId
                                        };
                                    })
                                .ToList()
                        })
                    .OrderBy(tribe => tribe.Name)
                    .ToList();


            LogService.Info(
                $"SAVE DATA | Tribus détectées par le parseur : {tribes.Count}");


            return new ArkSaveDataResult
            {
                Success = true,
                Tribes = tribes
            };
        }

        public async Task<PlayerSaveDataResult> LoadPlayersAsync()
        {
            var save =
                await GetSaveAsync();

            if (save == null)
            {
                return new PlayerSaveDataResult
                {
                    Success = false,
                    Error = "Fichier monde indisponible."
                };
            }


            var players =
                save.Players.Values
                    .Where(player =>
                        player != null)
                    .Select(player =>
                        new PlayerSaveInfo
                        {
                            PlayerName =
                                player.PlayerName
                                ?? "",

                            CharacterName =
                                player.CharacterName
                                ?? "",

                            UniqueNetId =
                                player.UniqueNetId
                                ?? "",

                            PlayerDataId =
                                player.PlayerDataId?.ToString()
                                ?? "",

                            TribeName =
                                player.TribeName
                                ?? "",

                            Level =
                                (int)player.Level
                        })
                    .OrderBy(player =>
                        player.PlayerName)
                    .ToList();


            LogService.Info(
                $"SAVE DATA | Profils joueurs détectés : {players.Count}");


            return new PlayerSaveDataResult
            {
                Success = true,
                Players = players
            };
        }

        public async Task<DinoSaveDataResult> LoadDinosAsync()
        {
            var save =
                await GetSaveAsync();

            if (save == null)
            {
                return new DinoSaveDataResult
                {
                    ErrorMessage = "Fichier monde indisponible."
                };
            }


            LogService.Info(
                "SAVE DATA | Chargement des dinos...");

            LogService.Info(
                $"SAVE DATA | TamedCreatures : {save.TamedCreatures.Count}");



            var dinos =
                save.TamedCreatures.Values
                .Select(dino =>
                {
                    return new DinoSaveInfo
                    {
                        Species =
                            ArkSpeciesHelper.GetDisplayName(
                                System.IO.Path.GetFileNameWithoutExtension(
                                    dino.ClassName)
                                .Replace("_Character_BP", "")
                                .Replace("_C", "")),

                        Name =
                                dino.TamedName ?? "",

                            ClassName =
                                dino.ClassName ?? "",

                            DinoId =
                                dino.DinoId ?? 0,

                            Gender =
                                dino.IsFemale ? "♀" : "♂",

                            BaseLevel =
                                dino.BaseLevel ?? 0,

                            Level =
                                dino.TotalLevel ?? 0,

                            Latitude =
                                dino.GPSLocation?.Y,

                            Longitude =
                                dino.GPSLocation?.X,

                            TribeId =
                                (int)dino.TribeId,

                            TribeName =
                                dino.TribeName ?? "",

                            OwnerName =
                                dino.TamerString ?? "",

                            FatherName =
                                dino.FatherName ?? "",

                            MotherName =
                                dino.MotherName ?? "",

                            MaleMutations =
                                dino.MutationsMale,

                            FemaleMutations =
                                dino.MutationsFemale,

                            IsCryopodded =
                                dino.IsCryo
                        };
                    })
                    .OrderBy(dino => dino.Species)
                    .ToList();

            LogService.Info(
                $"SAVE DATA | Dinos chargés : {dinos.Count}");

            return new DinoSaveDataResult
            {
                Dinos = dinos
            };
        }
    }
}