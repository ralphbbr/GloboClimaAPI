namespace GloboClimaAPI.Models
{
    public class ClimaDTO
    {
        public class PrevisaoClimatica
        {
            public string Name { get; set; }
            public Main Main { get; set; } 
            public List<Clima> clima { get; set; } 
        }

        public class Main
        {
            public float Temp { get; set; }
            public float Feels_like { get; set; } 
            public float Humidity { get; set; } 
        }

        public class Clima
        {
            public string Description { get; set; } 
            public string Icon { get; set; } 
        }
    }
}
