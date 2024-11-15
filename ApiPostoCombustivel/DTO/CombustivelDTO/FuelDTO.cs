using System.Text.Json.Serialization;

namespace ApiPostoCombustivel.DTO.CombustivelDTO
{
    public class FuelDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Stock { get; set; }
    }
}
