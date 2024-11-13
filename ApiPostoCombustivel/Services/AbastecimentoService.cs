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
            var combustivel = _combustivelRepository.GetCombustivelByTipo(abastecimentoDto.TipoCombustivel);
            if (combustivel == null)
            {
                throw new ArgumentException("Tipo de combustível não encontrado. Cadastre o combustível antes de realizar o abastecimento.");
            }

            if (abastecimentoDto.Quantidade <= 0)
            {
                throw new ArgumentException("A quantidade de abastecimento deve ser maior que zero.");
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



        //Só para deixar registrado que este método esta funcionando agora do jeito que eu quero, obrigado Deus
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

            var combustivelOriginal = _combustivelRepository.GetCombustivelByTipo(abastecimento.TipoCombustivel);
            if (combustivelOriginal == null)
            {
                throw new ArgumentException("Tipo de combustível original não encontrado.");
            }

            var tipoCombustivelAtualizado = abastecimento.TipoCombustivel;

            bool tipoCombustivelAlterado = updateDto.TipoCombustivel != null && updateDto.TipoCombustivel != abastecimento.TipoCombustivel;

            if (tipoCombustivelAlterado)
            {
                var novoCombustivel = _combustivelRepository.GetCombustivelByTipo(updateDto.TipoCombustivel);
                if (novoCombustivel == null)
                {
                    throw new ArgumentException("Novo tipo de combustível não encontrado.");
                }

                var quantidadeNecessaria = updateDto.Quantidade ?? abastecimento.Quantidade;
                if (novoCombustivel.Estoque < quantidadeNecessaria)
                {
                    throw new InvalidOperationException("Estoque insuficiente para o novo tipo de combustível.");
                }

                combustivelOriginal.Estoque += abastecimento.Quantidade;
                _combustivelRepository.UpdateCombustivel(combustivelOriginal);

                novoCombustivel.Estoque -= quantidadeNecessaria;
                _combustivelRepository.UpdateCombustivel(novoCombustivel);

                tipoCombustivelAtualizado = updateDto.TipoCombustivel;
            }

            if (updateDto.Quantidade.HasValue)
            {
                var diferencaQuantidade = updateDto.Quantidade.Value - abastecimento.Quantidade;


                if (!tipoCombustivelAlterado)
                {
                    if (diferencaQuantidade > 0 && combustivelOriginal.Estoque < diferencaQuantidade)
                    {
                        throw new InvalidOperationException("Estoque insuficiente para atualizar a quantidade de abastecimento.");
                    }

                    combustivelOriginal.Estoque -= diferencaQuantidade;
                    _combustivelRepository.UpdateCombustivel(combustivelOriginal);
                }

                abastecimento.Quantidade = updateDto.Quantidade.Value;
            }


            if (updateDto.Data.HasValue)
            {
                abastecimento.Data = updateDto.Data.Value;
            }


            abastecimento.TipoCombustivel = tipoCombustivelAtualizado;

            _abastecimentoRepository.UpdateAbastecimento(abastecimento);

            return AbastecimentoParser.ToDTO(abastecimento); 
        }

        public void DeleteAbastecimento(int id)
        {
            _abastecimentoRepository.DeleteAbastecimento(id);
        }
    }
}
