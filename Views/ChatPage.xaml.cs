using ArkPilot.Services;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;

namespace ArkPilot.Views
{
    public partial class ChatPage : Page
    {
        private readonly RconEngine rcon;

        private readonly AdminHistoryService adminHistory;

        private readonly DispatcherTimer chatTimer = new();

        private readonly Dictionary<string, DateTime> recentChatLines =
            new();

        public ChatPage(
            RconEngine engine,
            AdminHistoryService adminHistory)
        {
            InitializeComponent();

            rcon = engine;

            this.adminHistory =
                adminHistory;

            chatTimer.Interval =
    TimeSpan.FromSeconds(3);


            chatTimer.Tick +=
                ChatTimer_Tick;


            Loaded += async (_, __) =>
            {
                await RefreshChatAsync();

                chatTimer.Start();
            };


            Unloaded += (_, __) =>
            {
                chatTimer.Stop();
            };
        }


        // =========================
        // CHAT TIMER
        // =========================

        private async void ChatTimer_Tick(
            object? sender,
            EventArgs e)
        {
            await RefreshChatAsync();
        }


        // =========================
        // REFRESH CHAT
        // =========================

        private async Task RefreshChatAsync()
        {
            if (!rcon.IsConnected)
            {
                ChatStatusText.Text =
                    "🔴 RCON hors ligne";

                return;
            }


            string result =
                await rcon.Query(
                    "GetChat");


            if (result == "RCON_OFFLINE" ||
                result == "RCON_ERROR" ||
                result == "TIMEOUT")
            {
                ChatStatusText.Text =
                    "🔴 Chat indisponible";

                return;
            }



            foreach (string rawLine in result.Split(
                '\n',
                StringSplitOptions.RemoveEmptyEntries))
            {
                string line =
                    rawLine.Trim();

                if (line.Equals(
                    "Server received, But no response!!",
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }


                if (string.IsNullOrWhiteSpace(line))
                    continue;


                if (line.StartsWith(
                    "AdminCmd:",
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }


                DateTime now =
                    DateTime.Now;


                if (recentChatLines.TryGetValue(
                    line,
                    out DateTime lastSeen)
                    &&
                    now - lastSeen < TimeSpan.FromSeconds(5))
                {
                    continue;
                }


                recentChatLines[line] =
                    now;


                foreach (string oldLine in recentChatLines
                    .Where(item =>
                        now - item.Value > TimeSpan.FromMinutes(1))
                    .Select(item => item.Key)
                    .ToList())
                {
                    recentChatLines.Remove(
                        oldLine);
                }


                ChatList.Items.Add(
                    line);

                while (ChatList.Items.Count > 200)
                {
                    ChatList.Items.RemoveAt(0);
                }
            }


            ChatStatusText.Text =
                "🟢 Chat synchronisé";


            if (ChatList.Items.Count > 0)
            {
                ChatList.ScrollIntoView(
                    ChatList.Items[^1]);
            }
        }


        // =========================
        // BROADCAST
        // =========================

        private async void SendBroadcast_Click(
            object sender,
            RoutedEventArgs e)
        {
            string message =
                BroadcastMessageBox.Text.Trim();


            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(
                    "Veuillez saisir un message.",
                    "Broadcast",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            if (!rcon.IsConnected)
            {
                MessageBox.Show(
                    "Le RCON est déconnecté.\nImpossible d'envoyer le message.",
                    "Broadcast impossible",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            string result =
                await rcon.Query(
                    $"ServerChat [ArkPilot] {message}");


            LogService.Info(
                $"ADMIN | Broadcast : {message}");

            adminHistory.Add(
                "Broadcast",
                "Serveur",
                message);



            BroadcastMessageBox.Clear();


        }


        // =========================
        // QUICK MESSAGE
        // =========================

        private void QuickMessage_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;


            if (button.Tag is not string message)
                return;


            BroadcastMessageBox.Text =
                message;


            BroadcastMessageBox.Focus();

            BroadcastMessageBox.CaretIndex =
                BroadcastMessageBox.Text.Length;
        }


        // =========================
        // CHARACTER COUNT
        // =========================

        private void BroadcastMessageBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            int count =
                BroadcastMessageBox.Text.Length;


            CharacterCountText.Text =
                count <= 1
                    ? $"{count} caractère"
                    : $"{count} caractères";
        }
    }
}