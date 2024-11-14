using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class CombustivelExistenciaValidator
    {
        public static void ValidarCombustivelJaRegistrado(CombustivelRepository combustivelRepository, string tipo)
        {
            var combustivelExistente = combustivelRepository.GetCombustivelByTipo(tipo);
            if (combustivelExistente != null)
            {
                throw new CombustivelJaRegistradoException("Este tipo de combustível já está registrado.");
            }
        }

        public static void ValidarCombustivelExistente(CombustivelRepository combustivelRepository, int id)
        {
            var combustivel = combustivelRepository.GetCombustivelById(id);
            if (combustivel == null)
            {
                throw new CombustivelNaoEncontradoException("Combustível não encontrado.");
            }
        }
    }
}
