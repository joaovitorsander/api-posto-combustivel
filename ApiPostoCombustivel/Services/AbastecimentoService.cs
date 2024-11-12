using ApiPostoCombustivel.DTO.AbastecimentoDTO;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ApiPostoCombustivel.Services
{
    public class AbastecimentoService
    {
        private readonly IAbastecimentoRepository _abastecimentoRepository;
        private readonly ICombustivelRepository _combustivelRepository;

        public AbastecimentoService(IAbastecimentoRepository abastecimentoRepository, ICombustivelRepository combustivelRepository)
        {
            _abastecimentoRepository = abastecimentoRepository;
            _combustivelRepository = combustivelRepository;
        }

        public IEnumerable<AbastecimentoDTO> GetAbastecimentos()
        {
            var abastecimentos = _abastecimentoRepository.GetAbastecimentos();
            return abastecimentos.Select(AbastecimentoParser.ToDTO);
        }

        public AbastecimentoDTO GetAbastecimentoById(int id)
        {
            var abastecimento = _abastecimentoRepository.GetAbastecimentoById(id);
            return abastecimento != null ? AbastecimentoParser.ToDTO(abastecimento) : null;
        }

        public IEnumerable<AbastecimentoDTO> GetAbastecimentosByTipo(string tipoCombustivel)
        {
            var abastecimentos = _abastecimentoRepository.GetAbastecimentosByTipo(tipoCombustivel);
            return abastecimentos.Select(AbastecimentoParser.ToDTO);
        }

        public AbastecimentoDTO AddAbastecimento(AbastecimentoDTO abastecimentoDto)
        {
            var abastecimento = AbastecimentoParser.ToModel(abastecimentoDto);
            _abastecimentoRepository.AddAbastecimento(abastecimento);

            return AbastecimentoParser.ToDTO(abastecimento);
        }


        public AbastecimentoDTO UpdateAbastecimento(int id, UpdateAbastecimentoDTO updateDto)
        {
            var abastecimento = _abastecimentoRepository.GetAbastecimentoById(id);
            if (abastecimento == null)
            {
                throw new ArgumentException("Abastecimento não encontrado.");
            }

            if (updateDto.TipoCombustivel != null)
            {
                abastecimento.TipoCombustivel = updateDto.TipoCombustivel;
            }

            if (updateDto.Quantidade.HasValue)
            {
                abastecimento.Quantidade = updateDto.Quantidade.Value;
            }

            if (updateDto.Data.HasValue)
            {
                abastecimento.Data = updateDto.Data.Value;
            }

            _abastecimentoRepository.UpdateAbastecimento(abastecimento);
            return AbastecimentoParser.ToDTO(abastecimento);
        }


        public void DeleteAbastecimento(int id)
        {
            _abastecimentoRepository.DeleteAbastecimento(id);
        }
    }
}
