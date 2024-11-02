using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO;
using ApiPostoCombustivel.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Services
{
    public class CombustivelService
    {
        private readonly ICombustivelRepository _repository;

        public CombustivelService(ICombustivelRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CombustivelDTO> GetEstoque()
        {
            var combustiveis = _repository.GetEstoque();
            return combustiveis.Select(c => new CombustivelDTO { Tipo = c.Tipo, Quantidade = c.Quantidade });
        }

        public CombustivelDTO GetCombustivelByTipo(string tipo)
        {
            var combustivel = _repository.GetCombustivelByTipo(tipo);
            return combustivel != null ? new CombustivelDTO { Tipo = combustivel.Tipo, Quantidade = combustivel.Quantidade } : null;
        }

        public void AddCombustivel(CombustivelDTO combustivelDto)
        {
            var combustivel = new TbCombustivel
            {
                Tipo = combustivelDto.Tipo,
                Quantidade = combustivelDto.Quantidade
            };
            _repository.AddCombustivel(combustivel);
        }

        public void UpdateCombustivel(CombustivelDTO combustivelDto)
        {
            var combustivel = new TbCombustivel
            {
                Tipo = combustivelDto.Tipo,
                Quantidade = combustivelDto.Quantidade
            };
            _repository.UpdateCombustivel(combustivel);
        }

        public void DeleteCombustivel(string tipo)
        {
            _repository.DeleteCombustivel(tipo);
        }
    }
}
