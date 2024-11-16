using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class PriceValidator
    {
        // Valida os dados para a criação de um novo preço.
        // Verifica se o valor é válido e se a data de início foi fornecida.
        public static void ValidatePriceCreation(double value, DateTime startDate)
        {
            // Valida o valor do preço.
            ValidateValue(value);

            // Se a data de início for a data padrão (sem valor definido), lança uma exceção indicando que a data de início é obrigatória.
            if (startDate == default)
            {
                throw new StartDateRequiredException("Data de início é obrigatória para a criação do preço.");
            }
        }

        // Valida os dados para a edição de um preço existente.
        // Verifica se o valor foi alterado e, caso afirmativo, valida o novo valor.
        // Também valida se o período (data de início e fim) está correto.
        public static void ValidatePriceEdit(double? value, DateTime? startDate, DateTime? endDate)
        {
            // Se o valor foi alterado, valida o novo valor.
            if (value.HasValue)
            {
                ValidateValue(value.Value);
            }

            // Se tanto a data de início quanto a data de fim forem fornecidas, valida o período.
            if (startDate.HasValue && endDate.HasValue)
            {
                ValidatePeriod(startDate.Value, endDate.Value);
            }
        }

        // Valida o valor do preço para garantir que seja maior que zero.
        public static void ValidateValue(double value)
        {
            // Se o valor for menor ou igual a zero, lança uma exceção indicando que o valor do preço deve ser maior que zero.
            if (value <= 0)
            {
                throw new InvalidValueException("O valor do preço deve ser maior que zero.");
            }
        }

        // Valida o período, ou seja, a relação entre a data de início e a data de fim.
        // A data de início deve ser anterior ou igual à data de fim.
        public static void ValidatePeriod(DateTime? startDate, DateTime? endDate)
        {
            // Se a data de início for maior que a data de fim, lança uma exceção indicando que a data de início deve ser anterior ou igual à data de fim.
            if (startDate > endDate)
            {
                throw new InvalidPeriodException("A data de início deve ser anterior ou igual à data de fim.");
            }
        }
    }
}
