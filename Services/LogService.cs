using System;

namespace ArkPilot.Services
{
    public static class LogService
    {
        public static event Action<string>? OnLog;

        public static void Info(string message)
        {
            OnLog?.Invoke($"ℹ {DateTime.Now:HH:mm:ss} - {message}");
        }

        public static void Success(string message)
        {
            OnLog?.Invoke($"✔ {DateTime.Now:HH:mm:ss} - {message}");
        }

        public static void Warning(string message)
        {
            OnLog?.Invoke($"⚠ {DateTime.Now:HH:mm:ss} - {message}");
        }

        public static void Error(string message)
        {
            OnLog?.Invoke($"❌ {DateTime.Now:HH:mm:ss} - {message}");
        }

        public static void Error(Exception ex)
        {
            OnLog?.Invoke($"❌ {DateTime.Now:HH:mm:ss} - {ex.Message}");
        }
    }
}