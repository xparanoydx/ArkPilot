using System;
using System.Collections.Generic;

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

        public void Broadcast(string message)
        {
            _rcon.Send($"Broadcast {message}");
        }

        public void SaveWorld()
        {
            _rcon.Send("SaveWorld");
        }
    }
}