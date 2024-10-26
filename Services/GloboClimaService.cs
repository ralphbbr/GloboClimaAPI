using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using GloboClimaAPI.Models;
using static GloboClimaAPI.Models.ClimaDTO;
using System.Text.Json;

namespace GloboClimaAPI.Services
{
    public class GloboClimaService
    {

        private readonly HttpClient _httpClient;
        private const string ApiKey = "4e05e5c6f513bfeab2e6b2b39fe476be";

        public GloboClimaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PaisesDTO>> GetCountriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PaisesDTO>>("https://restcountries.com/v3.1/all");
        }

        public async Task<PaisesDTO> GetCountryAsync(string name)
        {
            try
            {
                // Faz a requisição para a API e espera a resposta
                var response = await _httpClient.GetFromJsonAsync<List<PaisesDTO>>($"https://restcountries.com/v3.1/name/{name}");

                // Verifica se a resposta contém algum país
                return response?.FirstOrDefault(); // Retorna o primeiro país encontrado ou nulo
            }
            catch (HttpRequestException e)
            {
                // Log ou manipulação de erro se a requisição falhar
                Console.WriteLine($"Erro ao chamar a API: {e.Message}");
                return null; // Retorna nulo se ocorrer um erro
            }
            catch (JsonException e)
            {
                // Log ou manipulação de erro para problemas de desserialização
                Console.WriteLine($"Erro ao desserializar a resposta: {e.Message}");
                return null; // Retorna nulo se ocorrer um erro
            }
        }
        public async Task<WeatherResponse> GetWeatherByCityAsync(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={ApiKey}&units=metric"; // "units=metric" para Celsius
            var weatherRetorno = await _httpClient.GetFromJsonAsync<WeatherResponse>(url);
            return weatherRetorno;
        }
    }
}
