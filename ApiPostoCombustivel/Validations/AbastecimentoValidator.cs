using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class AbastecimentoValidator
    {
        public static void ValidarQuantidade(double quantidade)
        {
            if (quantidade <= 0)
            {
                throw new QuantidadeAbastecimentoInvalidaException("A quantidade de abastecimento deve ser maior que zero.");
            }
        }

        public static void ValidarCombustivelExistente(CombustivelRepository combustivelRepository, string tipoCombustivel)
        {
            var combustivel = combustivelRepository.GetCombustivelByTipo(tipoCombustivel);
            if (combustivel == null)
            {
                throw new CombustivelNaoEncontradoException("Tipo de combustível não encontrado. Cadastre o combustível antes de realizar o abastecimento.");
            }
        }

        public static void ValidarEstoqueSuficiente(double estoque, double quantidadeNecessaria)
        {
            if (estoque < quantidadeNecessaria)
            {
                throw new EstoqueInsuficienteException("Estoque insuficiente para realizar o abastecimento.");
            }
        }

        public static void ValidarAbastecimentoExistente(AbastecimentoRepository abastecimentoRepository, int id)
        {
            var abastecimento = abastecimentoRepository.GetAbastecimentoById(id);
            if (abastecimento == null)
            {
                throw new AbastecimentoNaoEncontradoException("Abastecimento não encontrado.");
            }
        }
    }
}
