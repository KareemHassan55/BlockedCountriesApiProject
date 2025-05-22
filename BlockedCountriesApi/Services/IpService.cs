using BlockedCountriesApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BlockedCountriesApi.Services
{
    public class IpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public IpService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

  

        public async Task<IpLookupResponse?> LookupIpAsync(string ip)
        {
            var apiKey = "3f63caa81d094af9a9a345ad4b38a57d";  
            var url = $"https://api.ipgeolocation.io/ipgeo?apiKey={apiKey}&ip={ip}";

            using var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();

                var result = System.Text.Json.JsonSerializer.Deserialize<IpLookupResponse>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }
            catch
            {
                return null;
            }
        }

    }
}
