using System.Threading.Tasks;
using ArkPilot.Config;
using ArkPilot.Services;

namespace ArkPilot.Managers
{
    public class ServerManager
    {
        public RconClient RconClient { get; }
        public RconEngine RconEngine { get; }
        public ArkService ArkService { get; }
        public ServerMonitor Monitor { get; }

        public ServerManager(ServerConfig config)
        {
            RconClient = new RconClient();

            RconEngine = new RconEngine(RconClient);

            ArkService = new ArkService(RconEngine);

            Monitor = new ServerMonitor(
                RconEngine,
                config.ServerIp,
                config.RconPort,
                config.RconPassword);
        }

        public async Task<bool> ConnectAsync(ServerConfig config)
        {
            bool connected = await RconEngine.Connect(
                config.ServerIp,
                config.RconPort,
                config.RconPassword);

            if (connected)
            {
                Monitor.Start();
            }

            return connected;
        }

        public void Stop()
        {
            Monitor.Stop();
            RconEngine.Stop();
        }
    }
}