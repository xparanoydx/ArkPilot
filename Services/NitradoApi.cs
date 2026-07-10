using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ArkPilot.Services
{
    public class NitradoApi
    {
        private readonly string token;
        private readonly string serviceId;

        private HttpClient client;

        public NitradoApi(string token, string serviceId)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("La clé API Nitrado est vide.", nameof(token));

            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Le Service ID Nitrado est vide.", nameof(serviceId));

            this.token = token;
            this.serviceId = serviceId;

            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> GetStatus()
        {
            var url = $"https://api.nitrado.net/services/{serviceId}/gameservers";
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> StartServer()
        {
            var url = $"https://api.nitrado.net/services/{serviceId}/gameservers/restart";
            var response = await client.PostAsync(url, null);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> StopServer()
        {
            var url = $"https://api.nitrado.net/services/{serviceId}/gameservers/stop";
            var response = await client.PostAsync(url, null);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RestartServer()
        {
            var url = $"https://api.nitrado.net/services/{serviceId}/gameservers/restart";
            var response = await client.PostAsync(url, null);
            return await response.Content.ReadAsStringAsync();
        }
    }
}