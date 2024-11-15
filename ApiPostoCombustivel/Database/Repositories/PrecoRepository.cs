using ApiPostoCombustivel.Database.Models;

namespace ApiPostoCombustivel.Database.Repositories
{
    public class PrecoRepository
    {
        private readonly AppDbContext _context;

        public PrecoRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TbPreco> GetPrecos()
        {
            return _context.Precos.ToList();
        }

        public TbPreco GetPrecoById(int id)
        {
            return _context.Precos.FirstOrDefault(p => p.Id == id);
        }

        public TbPreco GetPrecoByCombustivelId(int combustivelId)
        {
            return _context.Precos.FirstOrDefault(p => p.CombustivelId == combustivelId);
        }

        public void AddPreco(TbPreco preco)
        {
            _context.Precos.Add(preco);
            _context.SaveChanges();
        }

        public void UpdatePreco(TbPreco preco)
        {
            var existingPreco = _context.Precos.FirstOrDefault(p => p.Id == preco.Id);
            if (existingPreco != null)
            {
                existingPreco.PrecoPorLitro = preco.PrecoPorLitro;
                existingPreco.DataInicio = preco.DataInicio;
                existingPreco.DataFim = preco.DataFim;
                existingPreco.CombustivelId = preco.CombustivelId;
                _context.SaveChanges();
            }
        }

        public void DeletePreco(int id)
        {
            var preco = _context.Precos.FirstOrDefault(p => p.Id == id);
            if (preco != null)
            {
                _context.Precos.Remove(preco);
                _context.SaveChanges();
            }
        }
    }
}
