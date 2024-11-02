namespace ApiPostoCombustivel.Database.Models
{
    public class TbAbastecimento
    {
        public int Id { get; set; }
        public string TipoCombustivel { get; set; }
        public double Quantidade { get; set; }
        public DateTime Data { get; set; }
    }
}
