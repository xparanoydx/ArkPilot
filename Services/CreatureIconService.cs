using ArkPilot.Data;
using System;
using System.IO;

namespace ArkPilot.Services
{
    public static class CreatureIconService
    {
        private const string RelativeFolder =
            "Assets/Icons/Creatures";

        public static string GetIconPath(
            string species)
        {
            string baseFolder =
                AppContext.BaseDirectory;

            string iconFolder =
                Path.Combine(
                    baseFolder,
                    RelativeFolder);

            string unknownPath =
                Path.Combine(
                    iconFolder,
                    "Unknown.png");

            if (string.IsNullOrWhiteSpace(species))
            {
                return unknownPath;
            }

            string iconPath =
                Path.Combine(
                    iconFolder,
                    $"{species}.png");

            return File.Exists(iconPath)
                ? iconPath
                : unknownPath;
        }

        public static string GetIconPathFromClassName(string? className)
        {
            var creature = CreatureDatabase.GetByClassName(className);

            if (creature == null)
                return GetIconPath("Unknown");

            return GetIconPath(creature.Species);
        }
    }
}