namespace ApiPostoCombustivel.DTO.PrecoDTO
{
    public class PriceDTO
    {
        public int Id { get; set; }
        public int FuelId { get; set; }
        public double PricePerLiter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
