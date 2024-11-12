namespace ApiPostoCombustivel.DTO.AbastecimentoDTO
{
    public class CreateAbastecimentoDTO
    {
        public string TipoCombustivel { get; set; }
        public double Quantidade { get; set; }
        public DateTime Data { get; set; }
    }
}
