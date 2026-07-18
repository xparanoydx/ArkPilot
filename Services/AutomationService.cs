using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using ArkPilot.Config;
using ArkPilot.Models;

namespace ArkPilot.Services
{
    public class AutomationService
    {
        private readonly RconEngine _rcon;

        private readonly ServerConfig _config;

        private readonly ArkEventService _arkEventService;

        private readonly NitradoService _nitradoService;

        private static readonly string RestartStateFile =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ArkPilot",
                "automation-restart-state.txt");

        private CancellationTokenSource? _cts;

        private bool _started;

        private DateTime? _lastRestartDate;

        private bool _restartInProgress;

        private DateTime _lastAutoSave =
        DateTime.Now;

        private bool? _weekendEventActive;

        private string? _lastWeekendConfigSignature;

        private bool _weekendEventOperationInProgress;

        private bool _weekendRestartRequired;

        private static readonly string WeekendStateFile =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ArkPilot",
                "weekend-event-state.txt");

        private static readonly string WeekendRestartRequiredStateFile =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ArkPilot",
                "weekend-restart-required-state.txt");


        public event Action<string>? OnLog;

        public event Action? WeekendEventStateChanged;

        public event Action? WeekendEventConfigApplied;

        public bool? WeekendEventActive =>
            _weekendEventActive;

        public bool WeekendRestartRequired =>
            _weekendRestartRequired;

        public bool WeekendEventOperationInProgress =>
            _weekendEventOperationInProgress;


        public AutomationService(
            RconEngine rcon,
            ServerConfig config)
        {
            _rcon = rcon;

            _config = config;

            _arkEventService =
                 new ArkEventService(
                     config);

            _nitradoService =
                new NitradoService(
                    config);

            _arkEventService.OnLog += message =>
                 OnLog?.Invoke(message);

            LoadRestartState();

            LoadWeekendState();

            LoadWeekendRestartRequiredState();

            _lastWeekendConfigSignature =
                GetWeekendConfigSignature();
        }

        // =========================
        // LOAD RESTART STATE
        // =========================

        private void LoadRestartState()
        {
            if (!File.Exists(RestartStateFile))
                return;


            string value =
                File.ReadAllText(RestartStateFile);


            if (DateTime.TryParse(
                value,
                out DateTime date))
            {
                _lastRestartDate =
                    date.Date;
            }
        }


        // =========================
        // SAVE WEEKEND STATE
        // =========================

        private void SaveWeekendState()
        {
            string? directory =
                Path.GetDirectoryName(
                    WeekendStateFile);


            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(
                    directory);
            }


            File.WriteAllText(
                WeekendStateFile,
                (_weekendEventActive ?? false).ToString());
        }


        // =========================
        // SAVE RESTART STATE
        // =========================

        private void SaveRestartState()
        {
            if (_lastRestartDate == null)
                return;


            string? directory =
                Path.GetDirectoryName(
                    RestartStateFile);


            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(
                    directory);
            }


            File.WriteAllText(
                RestartStateFile,
                _lastRestartDate.Value.ToString("yyyy-MM-dd"));
        }



        // =========================
        // LOAD WEEKEND STATE
        // =========================

        private void LoadWeekendState()
        {
            if (!File.Exists(WeekendStateFile))
                return;


            string value =
                File.ReadAllText(
                    WeekendStateFile);


            if (bool.TryParse(
                value,
                out bool active))
            {
                _weekendEventActive =
                    active;
            }
        }


        // =========================
        // LOAD WEEKEND RESTART REQUIRED STATE
        // =========================

        private void LoadWeekendRestartRequiredState()
        {
            if (!File.Exists(
                WeekendRestartRequiredStateFile))
            {
                return;
            }


            string value =
                File.ReadAllText(
                    WeekendRestartRequiredStateFile);


            if (bool.TryParse(
                value,
                out bool restartRequired))
            {
                _weekendRestartRequired =
                    restartRequired;
            }
        }


        // =========================
        // SAVE WEEKEND RESTART REQUIRED STATE
        // =========================

        private void SaveWeekendRestartRequiredState()
        {
            string? directory =
                Path.GetDirectoryName(
                    WeekendRestartRequiredStateFile);


            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(
                    directory);
            }


            File.WriteAllText(
                WeekendRestartRequiredStateFile,
                _weekendRestartRequired.ToString());
        }


        // =========================
        // START
        // =========================

        public void Start()
        {
            if (_started)
                return;

            _started = true;

            _cts = new CancellationTokenSource();


            OnLog?.Invoke(
                "🤖 Service Automation démarré");


            _ = AutomationLoop(
                _cts.Token);
        }


        // =========================
        // LOOP
        // =========================

        private async Task AutomationLoop(
            CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    CheckAutoRestart();

                    CheckWeekendEvent();


                    await Task.Delay(
                        TimeSpan.FromMinutes(1),
                        token);


                    if (_config.AutoSaveEnabled &&
                        DateTime.Now - _lastAutoSave >=
                        TimeSpan.FromMinutes(
                            _config.AutoSaveIntervalMinutes))
                    {
                        RunAutoSave();

                        _lastAutoSave =
                            DateTime.Now;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    OnLog?.Invoke(
                        $"❌ Automation : erreur - {ex.Message}");


                    try
                    {
                        await Task.Delay(
                            TimeSpan.FromSeconds(30),
                            token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
        }


        // =========================
        // WEEKEND CONFIG SIGNATURE
        // =========================

        private string GetWeekendConfigSignature()
        {
            return string.Join(
                "|",
                _config.WeekendTamingEnabled,
                _config.WeekendTamingMultiplier,

                _config.WeekendHarvestEnabled,
                _config.WeekendHarvestMultiplier,

                _config.WeekendBabyMatureEnabled,
                _config.WeekendBabyMatureMultiplier,

                _config.WeekendXpEnabled,
                _config.WeekendXpMultiplier,

                _config.WeekendWildDinoFoodDrainEnabled,
                _config.WeekendWildDinoFoodDrainMultiplier,

                _config.WeekendBabyCuddleIntervalEnabled,
                _config.WeekendBabyCuddleIntervalMultiplier,

                _config.WeekendStartDay,
                _config.WeekendStartHour,
                _config.WeekendStartMinute,

                _config.WeekendEndDay,
                _config.WeekendEndHour,
                _config.WeekendEndMinute);
        }

        private bool IsWeekendEventPeriod()
        {
            DateTime now =
                DateTime.Now;


            int currentDay =
                (int)now.DayOfWeek;


            int currentMinutes =
                (currentDay * 24 * 60) +
                (now.Hour * 60) +
                now.Minute;


            int startMinutes =
                (_config.WeekendStartDay * 24 * 60) +
                (_config.WeekendStartHour * 60) +
                _config.WeekendStartMinute;


            int endMinutes =
                (_config.WeekendEndDay * 24 * 60) +
                (_config.WeekendEndHour * 60) +
                _config.WeekendEndMinute;


            if (startMinutes <= endMinutes)
            {
                return currentMinutes >= startMinutes &&
                       currentMinutes <= endMinutes;
            }


            return currentMinutes >= startMinutes ||
                   currentMinutes <= endMinutes;
        }


        // =========================
        // CHECK WEEKEND EVENT NOW
        // =========================

        public void CheckWeekendEventNow()
        {
            CheckWeekendEvent();
        }


        // =========================
        // CHECK WEEKEND EVENT
        // =========================

        private void CheckWeekendEvent()
        {
            if (!_config.WeekendEventEnabled)
            {
                if (_weekendEventActive == true)
                {
                    OnLog?.Invoke(
                        "🏁 Automation : événement week-end désactivé manuellement");


                    _ = DeactivateWeekendEventAsync();
                }


                return;
            }

            bool shouldBeActive =
                IsWeekendEventPeriod();

            string currentSignature =
                GetWeekendConfigSignature();

            if (shouldBeActive &&
                 _weekendEventActive == true &&
                 _lastWeekendConfigSignature != currentSignature)
            {
                OnLog?.Invoke(
                    "🎉 Automation : modification de l'événement week-end détectée");


                _ = ActivateWeekendEventAsync();

                return;
            }


            if (_weekendEventActive == shouldBeActive)
                return;


            if (shouldBeActive)
            {
                OnLog?.Invoke(
                    "🎉 Automation : événement week-end à activer");


                _ = ActivateWeekendEventAsync();
            }
            else
            {
                OnLog?.Invoke(
                    "🏁 Automation : événement week-end à désactiver");


                _ = DeactivateWeekendEventAsync();
            }
        }


        // =========================
        // ACTIVATE WEEKEND EVENT
        // =========================

        private async Task ActivateWeekendEventAsync()
        {
            if (_weekendEventOperationInProgress)
                return;


            _weekendEventOperationInProgress = true;


            try
            {
                bool success =
                    await _arkEventService.ApplyWeekendEventAsync();


                if (!success)
                {
                    OnLog?.Invoke(
                        "❌ Automation : activation de l'événement annulée");

                    return;
                }


                _weekendEventActive =
                    true;

                SaveWeekendState();

                _weekendRestartRequired =
                    true;

                SaveWeekendRestartRequiredState();

                WeekendEventStateChanged?.Invoke();

                WeekendEventConfigApplied?.Invoke();


                _lastWeekendConfigSignature =
                    GetWeekendConfigSignature();


                OnLog?.Invoke(
                    "🔄 Automation : redémarrage manuel requis pour appliquer l'événement");

            }
            finally
            {
                _weekendEventOperationInProgress = false;
            }
        }

        // =========================
        // DEACTIVATE WEEKEND EVENT
        // =========================

        private async Task DeactivateWeekendEventAsync()
        {
            if (_weekendEventOperationInProgress)
                return;


            _weekendEventOperationInProgress = true;


            try
            {
                bool success =
                    await _arkEventService.RestoreOriginalConfigAsync();


                if (!success)
                {
                    OnLog?.Invoke(
                        "❌ Automation : désactivation de l'événement annulée");

                    return;
                }


                _weekendEventActive =
                    false;

                SaveWeekendState();

                _weekendRestartRequired =
                    true;

                SaveWeekendRestartRequiredState();

                WeekendEventStateChanged?.Invoke();


                OnLog?.Invoke(
                    "✅ Automation : événement week-end désactivé");


                OnLog?.Invoke(
                    "🔄 Automation : redémarrage manuel requis pour restaurer la configuration normale");

            }
            finally
            {
                _weekendEventOperationInProgress = false;
            }
        }


        // =========================
        // AUTO SAVE
        // =========================

        private void RunAutoSave()
        {
            OnLog?.Invoke(
                "💾 Automation : sauvegarde du serveur");


            _rcon.Send(
                "saveworld");
        }


        // =========================
        // GET ORIGINAL EVENT CONFIG
        // =========================

        public OriginalEventConfig? GetOriginalEventConfig()
        {
            return _arkEventService
                .GetOriginalEventConfig();
        }


        // =========================
        // SET CURRENT CONFIG AS ORIGINAL
        // =========================

        public async Task<bool> SetCurrentConfigAsOriginalAsync()
        {
            return await _arkEventService
                .SetCurrentConfigAsOriginalAsync();
        }


        // =========================
        // RELOAD CONFIG
        // =========================

        public void ReloadConfig()
        {
            ServerConfig newConfig =
                ConfigManager.Load();


            _config.AutoSaveEnabled =
                newConfig.AutoSaveEnabled;

            _config.AutoSaveIntervalMinutes =
                newConfig.AutoSaveIntervalMinutes;

            _config.AutoRestartEnabled =
                newConfig.AutoRestartEnabled;

            _config.AutoRestartHour =
                newConfig.AutoRestartHour;

            _config.AutoRestartMinute =
                newConfig.AutoRestartMinute;

            _config.WeekendEventEnabled =
                newConfig.WeekendEventEnabled;

            _config.WeekendTamingMultiplier =
                newConfig.WeekendTamingMultiplier;

            _config.WeekendHarvestMultiplier =
                newConfig.WeekendHarvestMultiplier;

            _config.WeekendBabyMatureMultiplier =
                newConfig.WeekendBabyMatureMultiplier;

            _config.WeekendXpMultiplier =
                newConfig.WeekendXpMultiplier;

            _config.WeekendWildDinoFoodDrainMultiplier =
                newConfig.WeekendWildDinoFoodDrainMultiplier;

            _config.WeekendBabyCuddleIntervalMultiplier =
                newConfig.WeekendBabyCuddleIntervalMultiplier;

            _config.WeekendTamingEnabled =
                newConfig.WeekendTamingEnabled;

            _config.WeekendHarvestEnabled =
                newConfig.WeekendHarvestEnabled;

            _config.WeekendBabyMatureEnabled =
                newConfig.WeekendBabyMatureEnabled;

            _config.WeekendXpEnabled =
                newConfig.WeekendXpEnabled;

            _config.WeekendWildDinoFoodDrainEnabled =
                newConfig.WeekendWildDinoFoodDrainEnabled;

            _config.WeekendBabyCuddleIntervalEnabled =
                newConfig.WeekendBabyCuddleIntervalEnabled;

            _config.WeekendStartDay =
                newConfig.WeekendStartDay;

            _config.WeekendStartHour =
                newConfig.WeekendStartHour;

            _config.WeekendStartMinute =
                newConfig.WeekendStartMinute;

            _config.WeekendEndDay =
                newConfig.WeekendEndDay;

            _config.WeekendEndHour =
                newConfig.WeekendEndHour;

            _config.WeekendEndMinute =
                newConfig.WeekendEndMinute;


            OnLog?.Invoke(
                "⚙ Automation : configuration rechargée");
        }


        // =========================
        // CLEAR WEEKEND RESTART REQUIRED
        // =========================

        public void ClearWeekendRestartRequired()
        {
            _weekendRestartRequired =
                false;

            SaveWeekendRestartRequiredState();

            WeekendEventStateChanged?.Invoke();


            OnLog?.Invoke(
                "✅ Automation : redémarrage manuel pris en compte");
        }

        // =========================
        // STOP
        // =========================

        public void Stop()
        {
            if (!_started)
                return;

            _started = false;


            _cts?.Cancel();

            _cts?.Dispose();

            _cts = null;


            OnLog?.Invoke(
                "🤖 Service Automation arrêté");
        }


        // =========================
        // CHECK AUTO RESTART
        // =========================

        private void CheckAutoRestart()
        {
            TimeSpan now =
                DateTime.Now.TimeOfDay;

            TimeSpan restartTime =
                new TimeSpan(
                _config.AutoRestartHour,
                _config.AutoRestartMinute,
                 0);


            if (_config.AutoRestartEnabled &&
                now >= restartTime &&
                now < restartTime.Add(
                    TimeSpan.FromMinutes(30)) &&
                _lastRestartDate != DateTime.Today)
            {
                if (_cts != null)
                {
                    _ = RunAutoRestart(
                        _cts.Token,
                        true);
                }
            }
        }


        // =========================
        // WAIT FOR NITRADO STATUS
        // =========================

        private async Task<bool> WaitForNitradoStatusAsync(
            string expectedStatus,
            TimeSpan timeout,
            CancellationToken token)
        {
            DateTime deadline =
                DateTime.Now.Add(timeout);


            while (DateTime.Now < deadline)
            {
                token.ThrowIfCancellationRequested();


                NitradoServerInfo info =
                    await _nitradoService.GetServerInfoAsync();


                if (info.Status.Equals(
                    expectedStatus,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }


                OnLog?.Invoke(
                    $"⏳ Automation : état Nitrado actuel - {info.Status}");


                await Task.Delay(
                    TimeSpan.FromSeconds(10),
                    token);
            }


            OnLog?.Invoke(
                $"❌ Automation : délai dépassé en attente de l'état {expectedStatus}");


            return false;
        }



        // =========================
        // AUTO RESTART
        // =========================

        private async Task RunAutoRestart(
            CancellationToken token,
            bool markDailyRestart = false)
        {
            if (_restartInProgress)
                return;


            _restartInProgress = true;


            try
            {
                OnLog?.Invoke(
                    "🔄 Automation : préparation du redémarrage automatique");


                _rcon.Send(
                    "Broadcast Redémarrage automatique du serveur dans 5 minutes");


                await Task.Delay(
                    TimeSpan.FromMinutes(4),
                    token);


                _rcon.Send(
                    "Broadcast Redémarrage automatique du serveur dans 1 minute");


                await Task.Delay(
                    TimeSpan.FromMinutes(1),
                    token);


                OnLog?.Invoke(
                    "💾 Automation : sauvegarde avant redémarrage");


                _rcon.Send(
                    "saveworld");


                await Task.Delay(
                    TimeSpan.FromSeconds(10),
                    token);


                OnLog?.Invoke(
                    "🔄 Automation : redémarrage du serveur via Nitrado");


                string restartResult =
                    await _nitradoService.RestartAsync();


                if (restartResult == "NITRADO_NOT_CONFIGURED" ||
                    restartResult == "NITRADO_ERROR")
                {
                    OnLog?.Invoke(
                        "❌ Automation : redémarrage Nitrado impossible");

                    return;
                }


                OnLog?.Invoke(
                    "✅ Automation : redémarrage Nitrado demandé");

                if (_weekendRestartRequired)
                {
                    ClearWeekendRestartRequired();
                }

                if (markDailyRestart)
                {
                    _lastRestartDate =
                        DateTime.Today;

                    SaveRestartState();
                }
            }
            catch (OperationCanceledException)
            {
                OnLog?.Invoke(
                    "🛑 Automation : redémarrage annulé");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke(
                    $"❌ Automation : erreur redémarrage - {ex.Message}");
            }
            finally
            {
                _restartInProgress = false;
            }
        }
    }
}