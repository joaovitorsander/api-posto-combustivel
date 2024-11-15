using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories.Interfaces;

namespace ApiPostoCombustivel.Database.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly AppDbContext _context;

        public PriceRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TbPrice> GetPrices()
        {
            return _context.Prices.ToList();
        }

        public TbPrice GetPriceById(int id)
        {
            return _context.Prices.FirstOrDefault(p => p.Id == id);
        }

        public TbPrice GetPriceByFuelId(int fuelId)
        {
            return _context.Prices.FirstOrDefault(p => p.FuelId == fuelId);
        }

        public void AddPrice(TbPrice price)
        {
            _context.Prices.Add(price);
            _context.SaveChanges();
        }

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
