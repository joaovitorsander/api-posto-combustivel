using System.Text.Json.Serialization;

namespace ApiPostoCombustivel.DTO.CombustivelDTO
{
    public class CombustivelDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public double Estoque { get; set; }
    }
}
