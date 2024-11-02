using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Repositories
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
            var existingCombustivel = _context.Combustiveis.FirstOrDefault(c => c.Tipo == combustivel.Tipo);
            if (existingCombustivel != null)
            {
                existingCombustivel.Quantidade = combustivel.Quantidade;
                _context.SaveChanges();
            }
        }

        public void DeleteCombustivel(string tipo)
        {
            var combustivel = _context.Combustiveis.FirstOrDefault(c => c.Tipo == tipo);
            if (combustivel != null)
            {
                _context.Combustiveis.Remove(combustivel);
                _context.SaveChanges();
            }
        }
    }
}
