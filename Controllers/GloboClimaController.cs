using GloboClimaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using GloboClimaAPI.Services;

namespace GloboClimaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GloboClimaController : ControllerBase
    {
        private readonly GloboClimaService _globoClimaService;

        public GloboClimaController( GloboClimaService globoClimaService)
        {
            _globoClimaService = globoClimaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaisesDTO>>> GetCountries()
        {
            var countries = await _globoClimaService.GetCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("pais/{nomePais}")]
        public async Task<ActionResult<PaisesDTO>> GetCountry(string nomePais)
        {
            // Chama o serviço para buscar informações sobre o país específico
            var countryInfo = await _globoClimaService.GetCountryAsync(nomePais);

            // Verifica se a informação do país foi encontrada
            if (countryInfo == null)
            {
                return NotFound($"País '{nomePais}' não encontrado ou sem informações disponíveis.");
            }

            return Ok(countryInfo);
        }

        [HttpGet("{cityName}")]
        public async Task<ActionResult<List<PaisesDTO>>> GetCountries(string cityName)
        {
            var countries = await _globoClimaService.GetWeatherByCityAsync(cityName);
            return Ok(countries);
        }

    }
    }

