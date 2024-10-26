using GloboClimaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using GloboClimaAPI.Services;
using System.Diagnostics.Metrics;
using static GloboClimaAPI.Services.DynamoDBService;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;

namespace GloboClimaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GloboClimaInternoController : ControllerBase
    {
        private readonly GloboClimaService _globoClimaService;
        private readonly DynamoDBService _dynamoDBService;

        public GloboClimaInternoController( GloboClimaService globoClimaService, DynamoDBService dynamoDBService)
        {
            _globoClimaService = globoClimaService;
            _dynamoDBService = dynamoDBService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Paises>>> BuscaPaisesFavoritos()
        {
            var countries = await _dynamoDBService.GetAllCountriesAsync();
            return Ok(countries);
        }

        [HttpPost]
        public async Task<ActionResult> PostPais([FromBody] Paises pais)
        {
            if (pais == null || string.IsNullOrEmpty(pais.Nome))
            {
                return BadRequest("Pais n�o pode ser nulo e deve ter um nome.");
            }

            // Chama a API para obter informa��es sobre o pa�s
            var countryInfo = await _globoClimaService.GetCountryAsync(pais.Nome);

            if (countryInfo == null)
            {
                return NotFound($"Informa��es sobre o pa�s '{pais.Nome}' n�o encontradas.");
            }

            // Verifica se o pa�s j� existe no DynamoDB
            if (await _dynamoDBService.PaisExistsAsync(pais.Nome))
            {
                return Conflict("Pais j� existe.");
            }

            // Salva o pa�s no DynamoDB
            await _dynamoDBService.SavePaisAsync(pais);
            return CreatedAtAction(nameof(BuscaPaisesFavoritos), new { nomePais = pais.Nome }, pais);
        }

        [HttpDelete("{nome}")]
        public async Task<ActionResult> DeletePais(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return BadRequest("O nome do pa�s deve ser fornecido.");
            }

            if (!await _dynamoDBService.PaisExistsAsync(nome))
            {
                return NotFound("Pais n�o encontrado.");
            }

            await _dynamoDBService.DeletePaisAsync(nome);
            return NoContent();
        }

        [HttpGet("{nome}")]
        public async Task<ActionResult<Paises>> ObterPais(string nome)
        {
            var pais = await _dynamoDBService.ObterPaisAsync(nome);
            if (pais == null) return NotFound();
            return Ok(pais);
        }

    }
    }

