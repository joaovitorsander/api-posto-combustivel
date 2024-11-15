namespace ApiPostoCombustivel.DTO.PrecoDTO
{
    public class CreatePriceDTO
    {
        public int FuelId { get; set; }
        public double PricePerLiter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
