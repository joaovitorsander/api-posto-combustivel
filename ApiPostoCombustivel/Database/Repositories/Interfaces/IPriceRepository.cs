using ApiPostoCombustivel.Database.Models;

namespace ApiPostoCombustivel.Database.Repositories.Interfaces
{
    public interface IPriceRepository
    {
        IEnumerable<TbPrice> GetPrices();
        TbPrice GetPriceById(int id);
        TbPrice GetPriceByFuelId(int combustivelId);
        void AddPrice(TbPrice preco);
        void UpdatePrice(TbPrice preco);
        void DeletePrice(int id);
    }
}
