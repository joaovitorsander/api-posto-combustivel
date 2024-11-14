using ApiPostoCombustivel.Database.Repositories;

namespace ApiPostoCombustivel.Validations
{
    public class CombustivelExistenciaValidator
    {
        public static void ValidarCombustivelJaRegistrado(CombustivelRepository combustivelRepository, string tipo)
        {
            var combustivelExistente = combustivelRepository.GetCombustivelByTipo(tipo);
            if (combustivelExistente != null)
            {
                throw new ArgumentException("Este tipo de combustível já está registrado.");
            }
        }

        public static void ValidarCombustivelExistente(CombustivelRepository combustivelRepository, int id)
        {
            var combustivel = combustivelRepository.GetCombustivelById(id);
            if (combustivel == null)
            {
                throw new ArgumentException("Combustível não encontrado.");
            }
        }
    }
}
