using GloboClimaAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static GloboClimaAPI.Models.ClimaDTO;


namespace GloboClimaAPI.Services
{
    public class GloboClimaService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "4e05e5c6f513bfeab2e6b2b39fe476be";
        public GloboClimaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
        }
        public async Task<PaisesDTO> BuscaPaisAsync(string name)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<PaisesDTO>>($"https://restcountries.com/v3.1/name/{name}");
                return response?.FirstOrDefault(); 
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro ao chamar a API: {e.Message}");
                return null; 
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Erro ao desserializar a resposta: {e.Message}");
                return null; 
            }
        }
        public async Task<PrevisaoClimatica> BuscaClimaCidadeAsync(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={ApiKey}&units=metric";
            var previsaoRetorno = await _httpClient.GetFromJsonAsync<PrevisaoClimatica>(url);
            return previsaoRetorno;
        }
}
}
