using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Database.Repositories.Interfaces;
using ApiPostoCombustivel.DTO.CombustivelDTO;
using ApiPostoCombustivel.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Services
{
    public class CombustivelService
    {
        private readonly CombustivelRepository _combustivelRepository;

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
            //Implementar validate
            if (!tiposValidos.Contains(combustivelDto.Tipo))
            {
                throw new ArgumentException("Tipo de combustível inválido.");
            }

            if (combustivelDto.Estoque <= 0)
            {
                throw new ArgumentException("O estoque deve ser maior que zero.");
            }

            var combustivelExistente = _combustivelRepository.GetCombustivelByTipo(combustivelDto.Tipo);
            if (combustivelExistente != null)
            {
                throw new ArgumentException("Este tipo de combustível já está registrado.");
            }

            var combustivel = CombustivelParser.ToModel(combustivelDto);
            _combustivelRepository.AddCombustivel(combustivel);
            return CombustivelParser.ToDTO(combustivel);
        }


        public void UpdateCombustivel(int id, UpdateCombustivelDTO updateDto)
        {
            var combustivel = _combustivelRepository.GetCombustivelById(id);
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

            _combustivelRepository.UpdateCombustivel(combustivel);
        }


        public void DeleteCombustivel(int id)
        {
            _combustivelRepository.DeleteCombustivel(id);
        }
    }
}
