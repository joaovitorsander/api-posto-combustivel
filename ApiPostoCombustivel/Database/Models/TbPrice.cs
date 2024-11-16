namespace ApiPostoCombustivel.Database.Models
{
    // Classe que representa a entidade "Preço" no banco de dados.
    // Utilizada para mapear os preços dos combustíveis em diferentes períodos.
    public class TbPrice
    {
        // Identificador único do preço. É a chave primária no banco de dados.
        public int Id { get; set; }

        // Relaciona o preço a um combustível específico pelo seu ID.
        // Isso ajuda a conectar os preços ao tipo de combustível correspondente.
        public int FuelId { get; set; }

        // Valor do preço por litro do combustível.
        // Este campo é essencial para calcular o custo total em abastecimentos.
        public double PricePerLiter { get; set; }

        // Data de início da validade do preço.
        // Serve para determinar a partir de quando este preço entra em vigor.
        public DateTime StartDate { get; set; }

        // Data de término da validade do preço (opcional).
        // Caso esteja nulo, indica que o preço ainda está em vigor.
        public DateTime? EndDate { get; set; }
    }
}
