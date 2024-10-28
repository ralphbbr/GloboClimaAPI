using GloboClimaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace GloboClimaAPI.Controllers
{
    [ApiController]
    //[Authorize]
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
        /// <summary>
        /// Busca todos os paises que foram adicionados como favoritos na base de dados. Exemplo de uso: /GloboClimaInterno
        /// </summary>
        /// <returns>a listagem de todos os paises presentes na tabela paisesFavoritos localizados na tabela de dados.</returns>
        [HttpGet]
        public async Task<ActionResult<List<PaisesModel>>> BuscaPaisesFavoritos()
        {
            var paisesFav = await _dynamoDBService.BuscaTodosPaisesFavoritosAsync();
            return Ok(paisesFav);
        }

        /// <summary>
        /// Busca todos as cidades que foram adicionadas como favoritas na base de dados. Exemplo de uso: /GloboClimaInterno/cidade
        /// </summary>
        /// <returns>a listagem de todas as cidades presentes na tabela cidadesFavoritas localizados na tabela de dados.</returns>
        [HttpGet("cidade")]
        public async Task<ActionResult<List<CityModel>>> BuscaCidadesFavoritas()
        {
            var cidadesFav = await _dynamoDBService.BuscaCidadesFavoritasAsync();
            return Ok(cidadesFav);
        }
        /// <summary>
        /// Adiciona uma cidade a tabela cidadesFavoritas na base de dados. Exemplo de uso: /GloboClimaInterno/cidade/belem
        /// </summary>
        /// <returns>retorna a confirma��o ou nega��o de que a cidade foi inserida na tabela de dados.</returns>
        [HttpPost("cidade/")]
        public async Task<ActionResult> SalvaCidade([FromBody] CityModel cidade)
        {
            if (cidade == null || string.IsNullOrEmpty(cidade.NomeCidade))
            {
                return BadRequest("Cidade n�o pode ser nulo e deve ter um nome.");
            }

            var cidadeInfo = await _globoClimaService.BuscaClimaCidadeAsync(cidade.NomeCidade);

            if (cidadeInfo == null)
            {
                return NotFound($"Informa��es sobre o cidade '{cidade.NomeCidade}' n�o encontradas.");
            }

            if (await _dynamoDBService.CidadeCadastradaAsync(cidade.NomeCidade))
            {
                return Conflict("Cidade j� existe.");
            }

            await _dynamoDBService.SalvaCidadeAsync(cidade);
            return CreatedAtAction(nameof(BuscaPaisesFavoritos), new { nomeCidade = cidade.NomeCidade }, cidade);
        }
        /// <summary>
        /// Adiciona um pais a tabela paisesFavoritos na base de dados. Exemplo de uso: /GloboClimaInterno/brasil
        /// </summary>
        /// <returns>retorna a confirma��o ou nega��o de que o pais foi inserido na tabela de dados.</returns>
        [HttpPost]
        public async Task<ActionResult> SalvaPais([FromBody] PaisesModel pais)
        {
            if (pais == null || string.IsNullOrEmpty(pais.Nome))
            {
                return BadRequest("Pais n�o pode ser nulo e deve ter um nome.");
            }

            var paisInfo = await _globoClimaService.BuscaPaisAsync(pais.Nome);

            if (paisInfo == null)
            {
                return NotFound($"Informa��es sobre o pa�s '{pais.Nome}' n�o encontradas.");
            }

            if (await _dynamoDBService.PaisCadastradoAsync(pais.Nome))
            {
                return Conflict("Pais j� existe.");
            }

            await _dynamoDBService.SalvaPaisAsync(pais);
            return CreatedAtAction(nameof(BuscaPaisesFavoritos), new { nomePais = pais.Nome }, pais);
        }
        /// <summary>
        /// Remove um pais da tabela paisesFavoritos na base de dados. Exemplo de uso: /GloboClimaInterno/brasil
        /// ATEN��O: � NECESSARIO QUE EXISTA O PAIS CADASTRADA NA TABELA PARA QUE DE SUCESSO
        /// </summary>
        /// <returns>retorna a confirma��o ou nega��o de que o pais foi removido na tabela de dados.</returns>
        [HttpDelete("{nome}")]
        public async Task<ActionResult> DeletePais(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return BadRequest("O nome do pa�s deve ser fornecido.");
            }

            if (!await _dynamoDBService.PaisCadastradoAsync(nome))
            {
                return NotFound("Pais n�o encontrado.");
            }

            await _dynamoDBService.DeletePaisAsync(nome);
            return NoContent();
        }
        /// <summary>
        /// Remove uma cidade da tabela cidadesFavoritas na base de dados. Exemplo de uso: /GloboClimaInterno/cidade/belem
        /// ATEN��O: � NECESSARIO QUE EXISTA A CIDADE CADASTRADA NA TABELA PARA QUE DE SUCESSO
        /// </summary>
        /// <returns>retorna a confirma��o ou nega��o de que a cidade foi removida na tabela de dados.</returns>
        [HttpDelete("cidade/{nome}")]
        public async Task<ActionResult> DeleteCidade(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return BadRequest("O nome do cidade deve ser fornecido.");
            }

            if (!await _dynamoDBService.CidadeCadastradaAsync(nome))
            {
                return NotFound("Cidade n�o encontrado.");
            }

            await _dynamoDBService.DeleteCidadeAsync(nome);
            return NoContent();
        }

    }
    }

