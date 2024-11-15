namespace ApiPostoCombustivel.Database.Models
{
    public class TbPreco
    {
        public int Id { get; set; }
        public int CombustivelId { get; set; } 
        public double PrecoPorLitro { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; } 
    }
}
