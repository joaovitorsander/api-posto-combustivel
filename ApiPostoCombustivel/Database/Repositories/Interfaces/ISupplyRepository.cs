using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;

namespace ApiPostoCombustivel.Database.Repositories.Interfaces
{
    public interface ISupplyRepository
    {
        IEnumerable<TbSupply> GetSupplies();
        TbSupply GetSupplyById(int id);
        IEnumerable<TbSupply> GetSuppliesByType(string tipoCombustivel);
        IEnumerable<TbSupply> GetSuppliesByDate(DateTime data);
        void AddSupply(TbSupply abastecimento);
        void UpdateSupply(TbSupply abastecimento);
        void DeleteSupply(int id);
    }
}
