using ArkPilot.Services;
using System.Windows;

namespace ArkPilot.Views
{
    public partial class PlayerDetailsWindow : Window
    {
        private readonly PlayerInfo player;

        private readonly RconEngine rcon;

        private readonly PlayerNoteService noteService;

        private readonly AdminHistoryService adminHistory;

        private readonly BannedPlayerService bannedPlayers;


        public PlayerDetailsWindow(
            PlayerInfo player,
            RconEngine rcon,
            PlayerNoteService noteService,
            AdminHistoryService adminHistory,
            BannedPlayerService bannedPlayers)
        {
            InitializeComponent();


            this.player =
                player;

            this.rcon =
                rcon;

            this.noteService =
                noteService;

            this.adminHistory =
                adminHistory;

            this.bannedPlayers =
                bannedPlayers;


            LoadPlayer();
        }


        // =========================
        // LOAD PLAYER
        // =========================

        private void LoadPlayer()
        {
            PlayerNameText.Text =
                player.Name;


            PlayerIdText.Text =
                player.Id;


            PlayerTribeText.Text =
                string.IsNullOrWhiteSpace(player.Tribe)
                    ? "--"
                    : player.Tribe;


            PlayerLevelText.Text =
                player.Level > 0
                    ? player.Level.ToString()
                    : "--";


            var note =
                noteService.GetNote(
                    player.Id);


            PlayerNoteBox.Text =
                note?.Note ?? "";
        }


        // =========================
        // SAVE NOTE
        // =========================

        private void SaveNote_Click(
            object sender,
            RoutedEventArgs e)
        {
            string note =
                PlayerNoteBox.Text.Trim();


            noteService.SaveNote(
                player.Id,
                player.Name,
                note);


            player.Note =
                note;


            LogService.Info(
                $"ADMIN | Note joueur enregistrée : {player.Name}");

            adminHistory.Add(
                "Note enregistrée",
                player.Name,
                note);


            MessageBox.Show(
                "La note a été enregistrée.",
                "Note joueur",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================
        // DELETE NOTE
        // =========================

        private void DeleteNote_Click(
            object sender,
            RoutedEventArgs e)
        {
            var existingNote =
                noteService.GetNote(
                    player.Id);


            if (existingNote == null)
            {
                MessageBox.Show(
                    "Ce joueur ne possède aucune note.",
                    "Suppression de note",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }


            var result =
                MessageBox.Show(
                    $"Voulez-vous vraiment supprimer la note de :\n\n{player.Name} ?",
                    "Confirmation suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
                return;


            noteService.DeleteNote(
                player.Id);


            player.Note = "";

            PlayerNoteBox.Clear();


            LogService.Info(
                $"ADMIN | Note joueur supprimée : {player.Name}");

            adminHistory.Add(
                "Note supprimée",
                player.Name,
                player.Id);


            MessageBox.Show(
                "La note a été supprimée.",
                "Note joueur",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }



        // =========================
        // KICK
        // =========================

        private void Kick_Click(
            object sender,
            RoutedEventArgs e)
        {
            var result =
                MessageBox.Show(
                    $"Voulez-vous vraiment expulser le joueur :\n\n{player.Name} ?",
                    "Confirmation Kick",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
                return;


            LogService.Warning(
                $"ADMIN | Kick joueur : {player.Name} ({player.Id})");

            adminHistory.Add(
                "Kick",
                player.Name,
                player.Id);


            rcon.Send(
                $"KickPlayer {player.Id}");


            MessageBox.Show(
                $"La commande Kick a été envoyée pour :\n\n{player.Name}",
                "Commande envoyée",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================
        // BAN
        // =========================

        private void Ban_Click(
            object sender,
            RoutedEventArgs e)
        {
            var result =
                MessageBox.Show(
                    $"Voulez-vous vraiment bannir le joueur :\n\n{player.Name} ?",
                    "Confirmation Ban",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
                return;


            LogService.Warning(
                $"ADMIN | Ban joueur : {player.Name} ({player.Id})");

            adminHistory.Add(
                "Ban",
                player.Name,
                player.Id);

            bannedPlayers.Add(
                player.Id,
                player.Name);


            rcon.Send(
                $"BanPlayer {player.Id}");


            MessageBox.Show(
                $"La commande Ban a été envoyée pour :\n\n{player.Name}",
                "Commande envoyée",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }



        // =========================
        // COPY ID
        // =========================

        private void CopyId_Click(
            object sender,
            RoutedEventArgs e)
        {
            Clipboard.SetText(
                player.Id);
        }


        // =========================
        // CLOSE
        // =========================

        private void Close_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}