using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;

namespace ApiPostoCombustivel.Database.Repositories.Interfaces
{
    // Define as operações que o repositório de combustíveis precisa ter.
    // A interface separa o que é necessário fazer de como será feito, facilitando alterações e testes.
    public interface IFuelRepository
    {
        // Retorna todos os combustíveis no estoque.
        // Útil para listar o inventário atual.
        IEnumerable<TbFuel> GetInventory();

        // Busca um combustível pelo ID, que é único.
        // Essencial para operações onde o identificador exato é necessário.
        TbFuel GetFuelById(int id);

        // Busca um combustível pelo tipo, como "Gasolina Comum".
        // Esse método ajuda em buscas mais intuitivas.
        TbFuel GetFuelByType(string tipo);

        // Adiciona um novo combustível no sistema.
        // Usado para registrar combustíveis que ainda não foram cadastrados.
        void AddFuel(TbFuel combustivel);

        // Atualiza informações de um combustível, como o estoque.
        // Necessário para manter os dados sempre corretos.
        void UpdateFuel(TbFuel combustivel);

        // Remove um combustível pelo ID.
        // Útil para situações em que o combustível não será mais utilizado.
        void DeleteFuel(int id);
    }
}
