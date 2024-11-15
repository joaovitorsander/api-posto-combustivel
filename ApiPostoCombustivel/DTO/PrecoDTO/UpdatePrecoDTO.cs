namespace ApiPostoCombustivel.DTO.PrecoDTO
{
    public class UpdatePrecoDTO
    {
        public int? CombustivelId { get; set; }
        public double? PrecoPorLitro { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
