using ApiPostoCombustivel.Database.Models;
using System.Collections.Generic;

namespace ApiPostoCombustivel.Repositories
{
    public interface ICombustivelRepository
    {
        IEnumerable<TbCombustivel> GetEstoque();
        TbCombustivel GetCombustivelByTipo(string tipo);
        void AddCombustivel(TbCombustivel combustivel);
        void UpdateCombustivel(TbCombustivel combustivel);
        void DeleteCombustivel(string tipo);
    }
}
