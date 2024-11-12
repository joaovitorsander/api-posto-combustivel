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
            if (abastecimentoDto.Quantidade <= 0)
            {
                throw new ArgumentException("A quantidade de abastecimento deve ser maior que zero.");
            }

            var combustivel = _combustivelRepository.GetCombustivelByTipo(abastecimentoDto.TipoCombustivel);
            if (combustivel == null)
            {
                throw new ArgumentException("Tipo de combustível não encontrado. Cadastre o combustível antes de realizar o abastecimento.");
            }

            if (combustivel.Estoque < abastecimentoDto.Quantidade)
            {
                throw new InvalidOperationException("Estoque insuficiente para realizar o abastecimento.");
            }

            combustivel.Estoque -= abastecimentoDto.Quantidade;
            _combustivelRepository.UpdateCombustivel(combustivel);

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

            if (updateDto.Quantidade.HasValue && updateDto.Quantidade <= 0)
            {
                throw new ArgumentException("A quantidade de abastecimento deve ser maior que zero.");
            }

            if (updateDto.TipoCombustivel != null)
            {
                var combustivel = _combustivelRepository.GetCombustivelByTipo(updateDto.TipoCombustivel);
                if (combustivel == null)
                {
                    throw new ArgumentException("Tipo de combustível não encontrado. Cadastre o combustível antes de atualizar o abastecimento.");
                }
            }

            if (updateDto.TipoCombustivel != null)
            {
                abastecimento.TipoCombustivel = updateDto.TipoCombustivel;
            }

            if (updateDto.Quantidade.HasValue)
            {
                var tipoCombustivel = updateDto.TipoCombustivel ?? abastecimento.TipoCombustivel;
                var combustivel = _combustivelRepository.GetCombustivelByTipo(tipoCombustivel);

                if (combustivel == null)
                {
                    throw new ArgumentException("Tipo de combustível não encontrado.");
                }

                var diferencaQuantidade = updateDto.Quantidade.Value - abastecimento.Quantidade;
                if (diferencaQuantidade > 0 && combustivel.Estoque < diferencaQuantidade)
                {
                    throw new InvalidOperationException("Estoque insuficiente para realizar a atualização do abastecimento.");
                }

                
                combustivel.Estoque -= diferencaQuantidade;
                _combustivelRepository.UpdateCombustivel(combustivel);

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
