using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class PrecoExistenciaValidator
    {
        public static void ValidarPrecoExistente(PrecoRepository precoRepository, int id)
        {
            var preco = precoRepository.GetPrecoById(id);
            if (preco == null)
            {
                throw new PrecoNaoEncontradoException("Preço não encontrado.");
            }
        }

        public static void ValidarDuplicidadePreco(PrecoRepository precoRepository, int combustivelId, DateTime dataInicio, DateTime? dataFim)
        {
            var precoDuplicado = precoRepository.GetPrecos()
                .Any(p => p.CombustivelId == combustivelId &&
                         ((dataFim.HasValue && p.DataFim.HasValue && p.DataFim >= dataInicio && p.DataInicio <= dataFim) ||
                          (!p.DataFim.HasValue && p.DataInicio <= dataInicio)));

            if (precoDuplicado)
            {
                throw new PrecoDuplicadoException("Já existe um preço registrado para o mesmo período.");
            }
        }

        public static void ValidarDataFimParaNovoPreco(PrecoRepository precoRepository, int combustivelId)
        {
            var ultimoPreco = precoRepository.GetPrecos()
                .Where(p => p.CombustivelId == combustivelId)
                .OrderByDescending(p => p.DataInicio)
                .FirstOrDefault();

            if (ultimoPreco != null && !ultimoPreco.DataFim.HasValue)
            {
                throw new DataFimNaoDefinidaException("Não é possível adicionar um novo preço sem que o preço anterior tenha uma DataFim definida.");
            }
        }
    }
}
