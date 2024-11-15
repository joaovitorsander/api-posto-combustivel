using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;

namespace ApiPostoCombustivel.Database.Repositories.Interfaces
{
    public interface IFuelRepository
    {
        IEnumerable<TbFuel> GetInventory();
        TbFuel GetFuelById(int id);
        TbFuel GetFuelByType(string tipo);
        void AddFuel(TbFuel combustivel);
        void UpdateFuel(TbFuel combustivel);
        void DeleteFuel(int id);
    }
}
