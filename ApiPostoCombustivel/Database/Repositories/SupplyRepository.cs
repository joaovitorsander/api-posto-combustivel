using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Database.Repositories
{
    public class SupplyRepository : ISupplyRepository
    {
        private readonly AppDbContext _context;

        public SupplyRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TbSupply> GetSupplies()
        {
            return _context.Supplies.ToList();
        }

        public TbSupply GetSupplyById(int id)
        {
            return _context.Supplies.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<TbSupply> GetSuppliesByType(string fuelType)
        {
            return _context.Supplies
                           .Where(a => a.FuelType == fuelType)
                           .ToList();
        }

        public void AddSupply(TbSupply supply)
        {
            _context.Supplies.Add(supply);
            _context.SaveChanges();
        }

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

        public void DeleteSupply(int id)
        {
            var supply = _context.Supplies.FirstOrDefault(a => a.Id == id);
            if (supply != null)
            {
                _context.Supplies.Remove(supply);
                _context.SaveChanges();
            }
        }

        public IEnumerable<TbSupply> GetSuppliesByDate(DateTime date)
        {
            return _context.Supplies
                           .Where(a => a.Date.Date == date.Date)
                           .ToList();
        }
    }
}
