using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkPilot.Services
{
    public class ArkService
    {
        private readonly RconEngine _rcon;

        public ArkService(RconEngine rcon)
        {
            _rcon = rcon;
        }

        // =========================
        // PLAYERS (V9 SAFE MODE)
        // =========================
        public Task<List<PlayerInfo>> GetPlayersAsync()
        {
            _rcon.Send("ListPlayers");

            // V9: pas de retour direct possible
            return Task.FromResult(new List<PlayerInfo>());
        }

        public void KickPlayer(string playerId)
        {
            _rcon.Send($"KickPlayer {playerId}");
        }

        public void BanPlayer(string playerId)
        {
            _rcon.Send($"BanPlayer {playerId}");
        }

        public async Task<string> BroadcastAsync(
            string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return "EMPTY_MESSAGE";

            return await _rcon.SendDirect(
                $"ServerChat ADMIN: {message.Trim()}");
        }


        public void SaveWorld()
        {
            _rcon.Send("SaveWorld");
        }
    }
}