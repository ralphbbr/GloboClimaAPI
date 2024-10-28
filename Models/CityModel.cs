using Amazon.DynamoDBv2.DataModel;

namespace GloboClimaAPI.Models
{
    [DynamoDBTable("cidadesFavoritas")]
    public class CityModel
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("cidade_nome")]
        public string NomeCidade { get; set; }
    }
}
