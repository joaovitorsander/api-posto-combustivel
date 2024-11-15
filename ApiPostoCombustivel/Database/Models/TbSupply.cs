namespace ApiPostoCombustivel.Database.Models
{
    public class TbSupply
    {
        public int Id { get; set; }
        public string FuelType { get; set; }
        public double Quantity { get; set; }
        public DateTime Date { get; set; }
        public double TotalValue { get; set; }
    }
}
