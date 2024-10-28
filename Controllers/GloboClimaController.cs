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
        /// <summary>
        /// Busca o pais de acordo com o nome fornecido, alguns paises como Cazaquistão só consegue realizar a busca em ingles. Exemplo de uso: /GloboClima/pais/Cazaquistao
        /// </summary>
        /// <returns>Os dados relacionados ao pais escolhido como por exemplo a moeda do pais.</returns>
        [HttpGet("pais/{pais}")]
        public async Task<ActionResult<PaisesDTO>> BuscaPais(string pais)
        {
            var infoDoPais = await _globoClimaService.BuscaPaisAsync(pais);

            if (infoDoPais == null)
            {
                return NotFound($"País '{pais}' não encontrado ou sem informações disponíveis.");
            }

            return Ok(infoDoPais);
        }
        /// <summary>
        /// Busca a cidade de acordo com o nome fornecido. Exemplo de uso: /GloboClima/belem
        /// </summary>
        /// <returns>Os dados climaticos relacionados a cidade escolhida como por exemplo a temperatura no momento.</returns>
        [HttpGet("{cidade}")]
        public async Task<ActionResult<List<PaisesDTO>>> BuscaCidade(string cidade)
        {
            var climaCidade = await _globoClimaService.BuscaClimaCidadeAsync(cidade);
            if (climaCidade == null)
            {
                return NotFound($"Cidade '{cidade}' não encontrado ou sem informações disponíveis.");
            }

            return Ok(climaCidade);
        }

    }
    }

