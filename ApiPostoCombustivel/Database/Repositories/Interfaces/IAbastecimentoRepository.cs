using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;

namespace ApiPostoCombustivel.Database.Repositories.Interfaces
{
    public interface IAbastecimentoRepository
    {
        IEnumerable<TbAbastecimento> GetAbastecimentos();
        TbAbastecimento GetAbastecimentoById(int id);
        IEnumerable<TbAbastecimento> GetAbastecimentosByTipo(string tipoCombustivel);
        IEnumerable<TbAbastecimento> GetAbastecimentosByData(DateTime data);
        void AddAbastecimento(TbAbastecimento abastecimento);
        void UpdateAbastecimento(TbAbastecimento abastecimento);
        void DeleteAbastecimento(int id);
    }
}
