using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Database.Repositories
{
    // Implementação do repositório de combustíveis, que segue o contrato definido pela interface IFuelRepository.
    // Essa classe é responsável por interagir diretamente com o banco de dados através do AppDbContext.
    public class FuelRepository : IFuelRepository
    {
        private readonly AppDbContext _context;

        // O contexto do banco de dados é injetado no construtor para gerenciar os dados.
        public FuelRepository(AppDbContext context)
        {
            _context = context;
        }

        // Retorna todos os combustíveis disponíveis no estoque.
        // Usei o método ToList para materializar a consulta do banco em memória.
        public IEnumerable<TbFuel> GetInventory()
        {
            return _context.Fuels.ToList();
        }

        // Busca um combustível específico pelo ID.
        // Este método retorna null se o combustível não for encontrado.
        public TbFuel GetFuelById(int id)
        {
            return _context.Fuels.FirstOrDefault(c => c.Id == id);
        }

        // Busca um combustível pelo tipo, como "Gasolina Comum".
        // Útil quando a busca por um tipo é mais intuitiva do que pelo ID.
        public TbFuel GetFuelByType(string type)
        {
            return _context.Fuels.FirstOrDefault(c => c.Type == type);
        }

        // Adiciona um novo combustível no banco de dados.
        // Após adicionar, o SaveChanges salva as alterações no banco.
        public void AddFuel(TbFuel fuel)
        {
            _context.Fuels.Add(fuel);
            _context.SaveChanges();
        }

        // Atualiza um combustível já existente.
        // Faz uma busca pelo ID e, se encontrado, atualiza os dados e salva as alterações.
        public void UpdateFuel(TbFuel fuel)
        {
            var existingCombustivel = _context.Fuels.FirstOrDefault(c => c.Id == fuel.Id);
            if (existingCombustivel != null)
            {
                existingCombustivel.Type = fuel.Type;
                existingCombustivel.Stock = fuel.Stock;
                _context.SaveChanges();
            }
        }

        // Remove um combustível do banco pelo ID.
        // Primeiro busca o combustível e, se encontrado, o remove e salva as alterações.
        public void DeleteFuel(int id)
        {
            var fuel = _context.Fuels.FirstOrDefault(c => c.Id == id);
            if (fuel != null)
            {
                _context.Fuels.Remove(fuel);
                _context.SaveChanges();
            }
        }
    }
}
