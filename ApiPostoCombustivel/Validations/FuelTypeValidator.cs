using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class FuelTypeValidator
    {
        // Lista que contém os tipos de combustíveis válidos para o sistema.
        private static readonly List<string> validTypes = new List<string>
        {
            "Gasolina Comum",        // Tipo de gasolina comum.
            "Gasolina Aditivada",    // Tipo de gasolina aditivada.
            "Gasolina Premium",      // Tipo de gasolina premium.
            "Diesel Comum",          // Tipo de diesel comum.
            "Diesel Aditivado",      // Tipo de diesel aditivado.
            "Etanol Comum",          // Tipo de etanol comum.
            "Etanol Aditivado"       // Tipo de etanol aditivado.
        };

        // Método que valida se o tipo de combustível informado é válido.
        // Se o tipo de combustível não estiver na lista de tipos válidos, uma exceção será lançada.
        public static void ValidateType(string type)
        {
            // Verifica se o tipo de combustível fornecido não está na lista de tipos válidos.
            if (!validTypes.Contains(type))
            {
                // Lança uma exceção indicando que o tipo de combustível é inválido.
                throw new InvalidFuelTypeException("Tipo de combustível inválido.");
            }
        }
    }
}
