namespace GloboClimaAPI.Models
{
    public class ClimaDTO
    {
        public class WeatherResponse
        {
            public string Name { get; set; } // Nome da cidade
            public Main Main { get; set; } // Informações sobre a temperatura e umidade
            public List<Clima> clima { get; set; } // Lista de condições meteorológicas
        }

        public class Main
        {
            public float Temp { get; set; } // Temperatura atual
            public float Feels_like { get; set; } // Sensação térmica
            public float Humidity { get; set; } // Umidade
        }

        public class Clima
        {
            public string Description { get; set; } // Descrição do tempo (ex: "clear sky")
            public string Icon { get; set; } // Código do ícone do clima
        }
    }
}
