namespace ApiPostoCombustivel.DTO.AbastecimentoDTO
{
    public class SupplyDTO
    {
        public int Id { get; set; }
        public string FuelType { get; set; }
        public double Quantity { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
