using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories.Interfaces;

namespace ApiPostoCombustivel.Database.Repositories
{
    // Implementação do repositório de preços, seguindo o contrato definido pela interface IPriceRepository.
    // Esta classe realiza operações diretas no banco de dados para gerenciar os registros de preços.
    public class PriceRepository : IPriceRepository
    {
        private readonly AppDbContext _context;

        // O contexto do banco de dados é injetado no construtor para permitir o acesso às tabelas.
        public PriceRepository(AppDbContext context)
        {
            _context = context;
        }

        // Retorna todos os preços cadastrados no banco de dados.
        // Usado para obter a lista completa de preços.
        public IEnumerable<TbPrice> GetPrices()
        {
            return _context.Prices.ToList();
        }

        // Busca um preço específico pelo ID.
        // Retorna null se o preço não for encontrado.
        public TbPrice GetPriceById(int id)
        {
            return _context.Prices.FirstOrDefault(p => p.Id == id);
        }

        // Busca um preço pelo ID do combustível.
        // Útil para relacionar preços a um tipo específico de combustível.
        public TbPrice GetPriceByFuelId(int fuelId)
        {
            return _context.Prices.FirstOrDefault(p => p.FuelId == fuelId);
        }

        // Adiciona um novo registro de preço ao banco de dados.
        // Após a inclusão, as alterações são salvas com SaveChanges.
        public void AddPrice(TbPrice price)
        {
            _context.Prices.Add(price);
            _context.SaveChanges();
        }

        // Atualiza um registro de preço existente.
        // Primeiro busca pelo ID e, se encontrado, atualiza os dados e salva as alterações.
        public void UpdatePrice(TbPrice price)
        {
            var existingPrice = _context.Prices.FirstOrDefault(p => p.Id == price.Id);
            if (existingPrice != null)
            {
                existingPrice.PricePerLiter = price.PricePerLiter;
                existingPrice.StartDate = price.StartDate;
                existingPrice.EndDate = price.EndDate;
                existingPrice.FuelId = price.FuelId;
                _context.SaveChanges();
            }
        }

        // Remove um registro de preço do banco pelo ID.
        // Busca o preço e, se encontrado, o remove e salva as alterações.
        public void DeletePrice(int id)
        {
            var price = _context.Prices.FirstOrDefault(p => p.Id == id);
            if (price != null)
            {
                _context.Prices.Remove(price);
                _context.SaveChanges();
            }
        }
    }
}
