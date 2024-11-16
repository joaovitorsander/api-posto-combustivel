namespace ApiPostoCombustivel.Database.Models
{
    // Classe que representa a entidade "Combustível" no banco de dados.
    // Ela é usada para mapear os dados de combustíveis e realizar operações relacionadas a eles.
    public class TbFuel
    {
        // Identificador único do combustível. É a chave primária no banco de dados.
        public int Id { get; set; }

        // Tipo do combustível, como "Gasolina Comum" ou "Diesel Aditivado".
        // Este campo é usado para categorizar os diferentes tipos de combustíveis disponíveis.
        public string Type { get; set; }

        // Quantidade disponível em estoque para este tipo de combustível.
        // Serve para acompanhar a disponibilidade no sistema.
        public double Stock { get; set; }
    }
}
