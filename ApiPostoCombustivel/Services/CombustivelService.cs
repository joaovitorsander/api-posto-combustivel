using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.DTO.CombustivelDTO;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Services
{
    public class CombustivelService
    {
        private readonly CombustivelRepository _combustivelRepository;

        public CombustivelService(AppDbContext context)
        {
            _combustivelRepository = new CombustivelRepository(context);
        }

        public IEnumerable<CombustivelDTO> GetEstoque()
        {
            var combustiveis = _combustivelRepository.GetEstoque();
            return combustiveis.Select(CombustivelParser.ToDTO);
        }

        public CombustivelDTO GetCombustivelById(int id)
        {
            var combustivel = _combustivelRepository.GetCombustivelById(id);
            return combustivel != null ? CombustivelParser.ToDTO(combustivel) : null;
        }

        public CombustivelDTO GetCombustivelByTipo(string tipo)
        {
            var combustivel = _combustivelRepository.GetCombustivelByTipo(tipo);
            return combustivel != null ? CombustivelParser.ToDTO(combustivel) : null;
        }

        public CombustivelDTO AddCombustivel(CombustivelDTO combustivelDto)
        {
            TipoCombustivelValidator.ValidarTipo(combustivelDto.Tipo);
            EstoqueCombustivelValidator.ValidarEstoque(combustivelDto.Estoque);
            CombustivelExistenciaValidator.ValidarCombustivelJaRegistrado(_combustivelRepository, combustivelDto.Tipo);

            var combustivel = CombustivelParser.ToModel(combustivelDto);
            _combustivelRepository.AddCombustivel(combustivel);
            return CombustivelParser.ToDTO(combustivel);
        }

        public void UpdateCombustivel(int id, UpdateCombustivelDTO updateDto)
        {
            CombustivelExistenciaValidator.ValidarCombustivelExistente(_combustivelRepository, id);

            var combustivel = _combustivelRepository.GetCombustivelById(id);

            if (updateDto.Tipo != null)
            {
                TipoCombustivelValidator.ValidarTipo(updateDto.Tipo);
                combustivel.Tipo = updateDto.Tipo;
            }

            if (updateDto.Estoque.HasValue)
            {
                EstoqueCombustivelValidator.ValidarEstoque(updateDto.Estoque);
                combustivel.Estoque = updateDto.Estoque.Value;
            }

            _combustivelRepository.UpdateCombustivel(combustivel);
        }

        public void DeleteCombustivel(int id)
        {
            CombustivelExistenciaValidator.ValidarCombustivelExistente(_combustivelRepository, id);
            _combustivelRepository.DeleteCombustivel(id);
        }
    }
}
