namespace ApiPostoCombustivel.Database.Models
{
    // Classe que representa a entidade "Abastecimento" no banco de dados.
    // Armazena informações sobre os abastecimentos realizados.
    public class TbSupply
    {
        // Identificador único do abastecimento. É a chave primária no banco de dados.
        public int Id { get; set; }

        // Tipo de combustível abastecido (ex.: "Gasolina Comum", "Diesel").
        // Esse campo é essencial para identificar o combustível usado no abastecimento.
        public string FuelType { get; set; }

        // Quantidade abastecida em litros.
        // Utilizado para calcular o total e controlar o estoque.
        public double Quantity { get; set; }

        // Data em que o abastecimento foi realizado.
        // Serve para relatórios e validações relacionadas a períodos.
        public DateTime Date { get; set; }

        // Valor total do abastecimento, calculado com base no preço do combustível.
        // Este campo armazena o custo final do abastecimento para relatórios e análises.
        public double TotalValue { get; set; }
    }
}
