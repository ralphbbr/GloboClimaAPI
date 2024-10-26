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
            // Chama o servi�o para buscar informa��es sobre o pa�s espec�fico
            var countryInfo = await _globoClimaService.GetCountryAsync(nomePais);

            // Verifica se a informa��o do pa�s foi encontrada
            if (countryInfo == null)
            {
                return NotFound($"Pa�s '{nomePais}' n�o encontrado ou sem informa��es dispon�veis.");
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

