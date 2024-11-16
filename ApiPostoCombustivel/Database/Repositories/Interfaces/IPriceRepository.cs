using ApiPostoCombustivel.Database.Models;

namespace ApiPostoCombustivel.Database.Repositories.Interfaces
{
    // Interface para gerenciar as operações relacionadas aos preços de combustíveis.
    // Usar uma interface ajuda a definir claramente os métodos necessários e facilita substituições ou testes.
    public interface IPriceRepository
    {
        // Método para retornar todos os preços registrados no sistema.
        // Isso é útil para listar todos os preços e exibir em relatórios ou consultas gerais.
        IEnumerable<TbPrice> GetPrices();

        // Busca um preço específico pelo ID.
        // Necessário para identificar um registro único, seja para edição ou exibição.
        TbPrice GetPriceById(int id);

        // Retorna o preço associado a um combustível específico.
        // Essencial para calcular valores com base no combustível selecionado.
        TbPrice GetPriceByFuelId(int combustivelId);

        // Adiciona um novo preço ao sistema.
        // Esse método é usado para inserir registros de novos preços.
        void AddPrice(TbPrice preco);

        // Atualiza as informações de um preço já registrado.
        // Importante para corrigir ou ajustar dados de preços existentes.
        void UpdatePrice(TbPrice preco);

        // Remove um preço do sistema usando seu ID.
        // Esse método é útil para exclusão de registros desatualizados ou inválidos.
        void DeletePrice(int id);
    }
}
