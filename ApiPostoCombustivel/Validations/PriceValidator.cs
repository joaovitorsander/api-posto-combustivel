using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class PriceValidator
    {
        public static void ValidatePriceCreation(double value, DateTime startDate)
        {
            ValidateValue(value);

            if (startDate == default)
            {
                throw new StartDateRequiredException("Data de início é obrigatória para a criação do preço.");
            }
        }

        public static void ValidatePriceEdit(double? value, DateTime? startDate, DateTime? endDate)
        {
            if (value.HasValue)
            {
                ValidateValue(value.Value);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                ValidatePeriod(startDate.Value, endDate.Value);
            }
        }

        public static void ValidateValue(double value)
        {
            if (value <= 0)
            {
                throw new InvalidValueException("O valor do preço deve ser maior que zero.");
            }
        }

        public static void ValidatePeriod(DateTime? startDate, DateTime? endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidPeriodException("A data de início deve ser anterior ou igual à data de fim.");
            }
        }
    }
}
