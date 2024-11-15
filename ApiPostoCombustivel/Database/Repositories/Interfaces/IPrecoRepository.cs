using ApiPostoCombustivel.Database.Models;

namespace ApiPostoCombustivel.Database.Repositories.Interfaces
{
    public interface IPrecoRepository
    {
        IEnumerable<TbPreco> GetPrecos();
        TbPreco GetPrecoById(int id);
        TbPreco GetPrecoByCombustivelId(int combustivelId);
        void AddPreco(TbPreco preco);
        void UpdatePreco(TbPreco preco);
        void DeletePreco(int id);
    }
}
