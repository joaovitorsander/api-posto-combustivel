using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;

namespace ApiPostoCombustivel.Repositories
{
    public interface IAbastecimentoRepository
    {
        IEnumerable<TbAbastecimento> GetAbastecimentos();
        TbAbastecimento GetAbastecimentoById(int id);
        IEnumerable<TbAbastecimento> GetAbastecimentosByTipo(string tipoCombustivel); // Novo método
        void AddAbastecimento(TbAbastecimento abastecimento);
        void UpdateAbastecimento(TbAbastecimento abastecimento);
        void DeleteAbastecimento(int id);
    }
}
