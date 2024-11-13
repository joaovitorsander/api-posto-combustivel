using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO.CombustivelDTO;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Services
{
    public class CombustivelService
    {
        private readonly ICombustivelRepository _repository;

        private readonly List<string> tiposValidos = new List<string>
        {
            "Gasolina Comum",
            "Gasolina Aditivada",
            "Gasolina Premium",
            "Diesel Comum",
            "Diesel Aditivado",
            "Etanol Comum",
            "Etanol Aditivado"
        };

        public CombustivelService(ICombustivelRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CombustivelDTO> GetEstoque()
        {
            var combustiveis = _repository.GetEstoque();
            return combustiveis.Select(CombustivelParser.ToDTO);
        }

        public CombustivelDTO GetCombustivelById(int id)
        {
            var combustivel = _repository.GetCombustivelById(id);
            return combustivel != null ? CombustivelParser.ToDTO(combustivel) : null;
        }

        public CombustivelDTO GetCombustivelByTipo(string tipo)
        {
            var combustivel = _repository.GetCombustivelByTipo(tipo);
            return combustivel != null ? CombustivelParser.ToDTO(combustivel) : null;
        }

        public CombustivelDTO AddCombustivel(CombustivelDTO combustivelDto)
        {
            if (!tiposValidos.Contains(combustivelDto.Tipo))
            {
                throw new ArgumentException("Tipo de combustível inválido.");
            }

            if (combustivelDto.Estoque <= 0)
            {
                throw new ArgumentException("O estoque deve ser maior que zero.");
            }

            var combustivelExistente = _repository.GetCombustivelByTipo(combustivelDto.Tipo);
            if (combustivelExistente != null)
            {
                throw new ArgumentException("Este tipo de combustível já está registrado.");
            }

            var combustivel = CombustivelParser.ToModel(combustivelDto);
            _repository.AddCombustivel(combustivel);
            return CombustivelParser.ToDTO(combustivel);
        }


        public void UpdateCombustivel(int id, UpdateCombustivelDTO updateDto)
        {
            var combustivel = _repository.GetCombustivelById(id);
            if (combustivel == null)
            {
                throw new ArgumentException("Combustível não encontrado.");
            }

            if (updateDto.Tipo != null)
            {
                if (!tiposValidos.Contains(updateDto.Tipo))
                {
                    throw new ArgumentException("Tipo de combustível inválido.");
                }
                combustivel.Tipo = updateDto.Tipo;
            }

            if (updateDto.Estoque.HasValue)
            {
                if (updateDto.Estoque.Value <= 0)
                {
                    throw new ArgumentException("O estoque deve ser maior que zero.");
                }
                combustivel.Estoque = updateDto.Estoque.Value;
            }

            _repository.UpdateCombustivel(combustivel);
        }


        public void DeleteCombustivel(int id)
        {
            _repository.DeleteCombustivel(id);
        }
    }
}
