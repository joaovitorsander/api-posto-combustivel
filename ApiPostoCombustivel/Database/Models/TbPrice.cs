namespace ApiPostoCombustivel.Database.Models
{
    public class TbPrice
    {
        public int Id { get; set; }
        public int FuelId { get; set; } 
        public double PricePerLiter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
    }
}
