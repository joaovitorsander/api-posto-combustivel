using ApiPostoCombustivel.Database.Models;

namespace ApiPostoCombustivel.Repositories
{
    public interface IAbastecimentoRepository
    {
        IEnumerable<TbAbastecimento> GetAbastecimentos();
        TbAbastecimento GetAbastecimentoById(int id);
        void AddAbastecimento(TbAbastecimento abastecimento);
        void UpdateAbastecimento(TbAbastecimento abastecimento);
        void DeleteAbastecimento(int id);
    }
}
