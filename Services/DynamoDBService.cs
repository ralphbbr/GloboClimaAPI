using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System.Diagnostics.Metrics;

namespace GloboClimaAPI.Services
{
    public class DynamoDBService
    {
        private readonly AmazonDynamoDBClient _client;

        public DynamoDBService()
        {
            // Configure o cliente para usar o DynamoDB Local
            var config = new AmazonDynamoDBConfig
            {
                ServiceURL = "http://localhost:8001"
            };

            _client = new AmazonDynamoDBClient(config);
        }
        private readonly IDynamoDBContext _context;
        public DynamoDBService(IAmazonDynamoDB dynamoDBClient)
        {
            _context = new DynamoDBContext(dynamoDBClient);
        }
        public async Task<List<Paises>> GetAllCountriesAsync()
        {
            var search = _context.ScanAsync<Paises>(new List<ScanCondition>());
            var results = await search.GetNextSetAsync();
            return results;
        }
        public async Task AddItemAsync(string nome)
        {
            var item = new Dictionary<string, AttributeValue>
        {
            { "nome", new AttributeValue { S = nome } }
        };

            var putItemRequest = new PutItemRequest
            {
                TableName = "paisesFavoritos",
                Item = item
            };

            await _client.PutItemAsync(putItemRequest);
        }

        public async Task DeletePaisAsync(string nome)
        {
            await _context.DeleteAsync<Paises>(nome);
        }
        public async Task<Paises> ObterPaisAsync(string nome)
        {
            return await _context.LoadAsync<Paises>(nome);
        }
        public async Task<List<Paises>> GetAllPaisesAsync()
        {
            return await _context.ScanAsync<Paises>(new List<ScanCondition>()).GetRemainingAsync();
        }
        public async Task<bool> PaisExistsAsync(string nome)
        {
            var pais = await _context.LoadAsync<Paises>(nome);
            return pais != null;
        }
        public async Task SavePaisAsync(Paises pais)
        {
            await _context.SaveAsync(pais);
        }

        [DynamoDBTable("paisesFavoritos")]
        public class Paises
        {
            [DynamoDBProperty("pais_nome")]
            public string Nome { get; set; }
        }


    }
}
