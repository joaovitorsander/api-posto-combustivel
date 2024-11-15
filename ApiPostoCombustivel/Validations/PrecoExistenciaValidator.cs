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

        public static void ValidarDuplicidadePreco(PrecoRepository precoRepository, int combustivelId, DateTime dataInicio, DateTime? dataFim, int? precoIdAtual = null)
        {
            var precos = precoRepository.GetPrecos()
                .Where(p => p.CombustivelId == combustivelId);

            if (precoIdAtual.HasValue)
            {
                precos = precos.Where(p => p.Id != precoIdAtual.Value);
            }

            var precoDuplicado = precos.Any(p =>
                (dataFim.HasValue && p.DataFim.HasValue && p.DataInicio <= dataFim && p.DataFim >= dataInicio) || 
                (!p.DataFim.HasValue && p.DataInicio <= dataInicio) ||
                (dataFim == null && p.DataFim >= dataInicio));

            if (precoDuplicado)
            {
                throw new PrecoDuplicadoException("Já existe um preço registrado que abrange o mesmo período ou datas conflitantes.");
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

        public static void ValidarDataFimParaAtualizacao(PrecoRepository precoRepository, int combustivelId, int precoId)
        {
            var precosFuturos = precoRepository.GetPrecos()
                .Where(p => p.CombustivelId == combustivelId && p.Id != precoId && p.DataInicio > DateTime.UtcNow)
                .ToList();

            if (precosFuturos.Any())
            {
                throw new DataFimNaoDefinidaException("Não é possível definir a DataFim como null enquanto existir um preço futuro registrado.");
            }
        }
    }
}
