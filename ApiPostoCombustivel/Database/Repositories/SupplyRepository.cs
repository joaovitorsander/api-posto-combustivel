using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Database.Repositories
{
    // Implementação do repositório de abastecimentos.
    // Esta classe realiza as operações no banco de dados relacionadas aos abastecimentos.
    public class SupplyRepository : ISupplyRepository
    {
        private readonly AppDbContext _context;

        // O contexto do banco de dados é injetado no construtor para que possamos acessar as tabelas.
        public SupplyRepository(AppDbContext context)
        {
            _context = context;
        }

        // Retorna todos os registros de abastecimentos.
        // Usado para listar todos os abastecimentos registrados.
        public IEnumerable<TbSupply> GetSupplies()
        {
            return _context.Supplies.ToList();
        }

        // Busca um abastecimento pelo ID.
        // Retorna null caso o abastecimento não seja encontrado.
        public TbSupply GetSupplyById(int id)
        {
            return _context.Supplies.FirstOrDefault(a => a.Id == id);
        }

        // Retorna todos os abastecimentos de um tipo específico de combustível.
        // Útil para filtrar por tipo de combustível.
        public IEnumerable<TbSupply> GetSuppliesByType(string fuelType)
        {
            return _context.Supplies
                           .Where(a => a.FuelType == fuelType)
                           .ToList();
        }

        // Adiciona um novo registro de abastecimento ao banco de dados.
        // Após a inclusão, as alterações são salvas.
        public void AddSupply(TbSupply supply)
        {
            _context.Supplies.Add(supply);
            _context.SaveChanges();
        }

        // Atualiza um registro de abastecimento existente.
        // Busca pelo ID e, se encontrado, altera os dados e salva as alterações.
        public void UpdateSupply(TbSupply supply)
        {
            var existingAbastecimento = _context.Supplies.FirstOrDefault(a => a.Id == supply.Id);
            if (existingAbastecimento != null)
            {
                existingAbastecimento.Quantity = supply.Quantity;
                existingAbastecimento.FuelType = supply.FuelType;
                _context.SaveChanges();
            }
        }

        // Remove um registro de abastecimento do banco pelo ID.
        // Busca o registro e, se encontrado, o remove e salva as alterações.
        public void DeleteSupply(int id)
        {
            var supply = _context.Supplies.FirstOrDefault(a => a.Id == id);
            if (supply != null)
            {
                _context.Supplies.Remove(supply);
                _context.SaveChanges();
            }
        }

        // Retorna todos os abastecimentos realizados em uma data específica.
        // Filtra os registros pela data informada.
        public IEnumerable<TbSupply> GetSuppliesByDate(DateTime date)
        {
            return _context.Supplies
                           .Where(a => a.Date.Date == date.Date)
                           .ToList();
        }
    }
}
