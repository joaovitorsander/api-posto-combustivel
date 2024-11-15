using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class PrecoValidator
    {
        public static void ValidarCriacaoPreco(double valor, DateTime dataInicio)
        {
            ValidarValor(valor);

            if (dataInicio == default)
            {
                throw new DataInicioObrigatoriaException("Data de início é obrigatória para a criação do preço.");
            }
        }

        public static void ValidarEdicaoPreco(double? valor, DateTime? dataInicio, DateTime? dataFim)
        {
            if (valor.HasValue)
            {
                ValidarValor(valor.Value);
            }

            if (dataInicio.HasValue && dataFim.HasValue)
            {
                ValidarPeriodo(dataInicio.Value, dataFim.Value);
            }
        }

        public static void ValidarValor(double valor)
        {
            if (valor <= 0)
            {
                throw new ValorInvalidoException("O valor do preço deve ser maior que zero.");
            }
        }

        public static void ValidarPeriodo(DateTime? dataInicio, DateTime? dataFim)
        {
            if (dataInicio > dataFim)
            {
                throw new PeriodoInvalidoException("A data de início deve ser anterior ou igual à data de fim.");
            }
        }
    }
}
