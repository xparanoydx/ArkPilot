using ArkPilot.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ArkPilot.Views
{
    public partial class ConsolePage : Page
    {
        private readonly RconEngine rcon;


        public ConsolePage(RconEngine engine)
        {
            InitializeComponent();

            rcon = engine;


            rcon.OnLog += Rcon_OnLog;

            rcon.OnResponse += Rcon_OnResponse;


            Unloaded += ConsolePage_Unloaded;
        }



        // =========================
        // LOG RCON
        // =========================

        private void Rcon_OnLog(string message)
        {
            Dispatcher.Invoke(() =>
            {
                AddLog(
                    $"[{DateTime.Now:HH:mm:ss}] {message}");
            });
        }



        // =========================
        // RESPONSE
        // =========================

        private void Rcon_OnResponse(
            string command,
            string response)
        {
            Dispatcher.Invoke(() =>
            {
                AddLog(
                    $"> {command}");

                AddLog(
                    response);
            });
        }



        private void AddLog(string text)
        {
            ConsoleList.Items.Add(text);


            if (ConsoleList.Items.Count > 300)
            {
                ConsoleList.Items.RemoveAt(0);
            }


            ConsoleList.ScrollIntoView(
                ConsoleList.Items[^1]);
        }



        // =========================
        // SEND COMMAND
        // =========================

        private void SendCommand_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                CommandBox.Text))
                return;


            string command =
                CommandBox.Text.Trim();


            AddLog(
                $"> {command}");


            rcon.Send(command);


            CommandBox.Clear();
        }



        // =========================
        // CLEAR
        // =========================

        private void Clear_Click(
            object sender,
            RoutedEventArgs e)
        {
            ConsoleList.Items.Clear();
        }



        // =========================
        // CLEANUP
        // =========================

        private void ConsolePage_Unloaded(
            object sender,
            RoutedEventArgs e)
        {
            rcon.OnLog -= Rcon_OnLog;

            rcon.OnResponse -= Rcon_OnResponse;
        }
    }
}