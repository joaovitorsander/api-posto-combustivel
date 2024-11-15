using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Database.Repositories
{
    public class FuelRepository : IFuelRepository
    {
        private readonly AppDbContext _context;

        public FuelRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TbFuel> GetInventory()
        {
            return _context.Fuels.ToList();
        }

        public TbFuel GetFuelById(int id)
        {
            return _context.Fuels.FirstOrDefault(c => c.Id == id);
        }

        public TbFuel GetFuelByType(string type)
        {
            return _context.Fuels.FirstOrDefault(c => c.Type == type);
        }

        public void AddFuel(TbFuel fuel)
        {
            _context.Fuels.Add(fuel);
            _context.SaveChanges();
        }

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
