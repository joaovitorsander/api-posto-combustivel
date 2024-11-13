using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Database.Repositories
{
    public class CombustivelRepository : ICombustivelRepository
    {
        private readonly AppDbContext _context;

        public CombustivelRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TbCombustivel> GetEstoque()
        {
            return _context.Combustiveis.ToList();
        }

        public TbCombustivel GetCombustivelById(int id)
        {
            return _context.Combustiveis.FirstOrDefault(c => c.Id == id);
        }

        public TbCombustivel GetCombustivelByTipo(string tipo)
        {
            return _context.Combustiveis.FirstOrDefault(c => c.Tipo == tipo);
        }

        public void AddCombustivel(TbCombustivel combustivel)
        {
            _context.Combustiveis.Add(combustivel);
            _context.SaveChanges();
        }

        public void UpdateCombustivel(TbCombustivel combustivel)
        {
            var existingCombustivel = _context.Combustiveis.FirstOrDefault(c => c.Id == combustivel.Id);
            if (existingCombustivel != null)
            {
                existingCombustivel.Tipo = combustivel.Tipo;
                existingCombustivel.Estoque = combustivel.Estoque;
                _context.SaveChanges();
            }
        }

        public void DeleteCombustivel(int id)
        {
            var combustivel = _context.Combustiveis.FirstOrDefault(c => c.Id == id);
            if (combustivel != null)
            {
                _context.Combustiveis.Remove(combustivel);
                _context.SaveChanges();
            }
        }
    }
}
