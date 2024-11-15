using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class FuelTypeValidator
    {
        private static readonly List<string> validTypes = new List<string>
        {
            "Gasolina Comum",
            "Gasolina Aditivada",
            "Gasolina Premium",
            "Diesel Comum",
            "Diesel Aditivado",
            "Etanol Comum",
            "Etanol Aditivado"
        };

        public static void ValidateType(string type)
        {
            if (!validTypes.Contains(type))
            {
                throw new InvalidFuelTypeException("Tipo de combustível inválido.");
            }
        }
    }
}
