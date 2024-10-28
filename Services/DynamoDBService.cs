using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using GloboClimaAPI.Models;

namespace GloboClimaAPI.Services
{
    public class DynamoDBService
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly IDynamoDBContext _context;
        public DynamoDBService(IAmazonDynamoDB dynamoDBClient)
        {
            _context = new DynamoDBContext(dynamoDBClient);
        }
        public async Task<List<PaisesModel>> BuscaTodosPaisesFavoritosAsync()
        {
            var search = _context.ScanAsync<PaisesModel>(new List<ScanCondition>());
            var results = await search.GetNextSetAsync();
            return results;
        }
        public async Task<List<CityModel>> BuscaCidadesFavoritasAsync()
        {
            var search = _context.ScanAsync<CityModel>(new List<ScanCondition>());
            var results = await search.GetNextSetAsync();
            return results;
        }
        public async Task DeletePaisAsync(string nome)
        {
            await _context.DeleteAsync<PaisesModel>(nome);
        }
        public async Task DeleteCidadeAsync(string nome)
        {
            await _context.DeleteAsync<CityModel>(nome);
        }
        public async Task<PaisesModel> ObterPaisAsync(string nome)
        {
            return await _context.LoadAsync<PaisesModel>(nome);
        }
        public async Task<List<PaisesModel>> GetAllPaisesAsync()
        {
            return await _context.ScanAsync<PaisesModel>(new List<ScanCondition>()).GetRemainingAsync();
        }
        public async Task<bool> PaisCadastradoAsync(string nome)
        {
            var pais = await _context.LoadAsync<PaisesModel>(nome);
            return pais != null;
        }
        public async Task<bool> CidadeCadastradaAsync(string nome)
        {
            var cidade = await _context.LoadAsync<CityModel>(nome);
            return cidade != null;
        }
        public async Task SalvaPaisAsync(PaisesModel pais)
        {
            await _context.SaveAsync(pais);
        }
        public async Task SalvaCidadeAsync(CityModel cidade)
        {
            await _context.SaveAsync(cidade);
        }
    }
}
