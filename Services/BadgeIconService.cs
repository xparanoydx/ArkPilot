using System;
using System.IO;

namespace ArkPilot.Services
{
    public static class BadgeIconService
    {
        public static string? GetBadgePath(
            bool isTek,
            bool isAberrant,
            bool isXCreature,
            bool isRCreature,
            bool isBoss,
            bool isFantasy)
        {
            string? file = null;

            if (isBoss)
                file = "Boss.png";
            else if (isTek)
                file = "Tek.png";
            else if (isAberrant)
                file = "Aberrant.png";
            else if (isXCreature)
                file = "XCreature.png";
            else if (isRCreature)
                file = "RCreature.png";
            else if (isFantasy)
                file = "Fantasy.png";

            if (file == null)
                return null;

            string path = Path.Combine(
                AppContext.BaseDirectory,
                "Assets",
                "Icons",
                "Badges",
                file);

            return File.Exists(path)
                ? path
                : null;
        }
    }
}