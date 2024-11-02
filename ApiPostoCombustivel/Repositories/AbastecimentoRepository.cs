using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Repositories
{
    public class AbastecimentoRepository : IAbastecimentoRepository
    {
        private readonly AppDbContext _context;

        public AbastecimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TbAbastecimento> GetAbastecimentos()
        {
            return _context.Abastecimentos.ToList();
        }

        public TbAbastecimento GetAbastecimentoById(int id)
        {
            return _context.Abastecimentos.FirstOrDefault(a => a.Id == id);
        }

        public void AddAbastecimento(TbAbastecimento abastecimento)
        {
            _context.Abastecimentos.Add(abastecimento);
            _context.SaveChanges();
        }

        public void UpdateAbastecimento(TbAbastecimento abastecimento)
        {
            var existingAbastecimento = _context.Abastecimentos.FirstOrDefault(a => a.Id == abastecimento.Id);
            if (existingAbastecimento != null)
            {
                existingAbastecimento.Quantidade = abastecimento.Quantidade;
                existingAbastecimento.TipoCombustivel = abastecimento.TipoCombustivel;
                _context.SaveChanges();
            }
        }

        public void DeleteAbastecimento(int id)
        {
            var abastecimento = _context.Abastecimentos.FirstOrDefault(a => a.Id == id);
            if (abastecimento != null)
            {
                _context.Abastecimentos.Remove(abastecimento);
                _context.SaveChanges();
            }
        }
    }
}
