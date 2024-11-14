using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class TipoCombustivelValidator
    {
        private static readonly List<string> tiposValidos = new List<string>
        {
            "Gasolina Comum",
            "Gasolina Aditivada",
            "Gasolina Premium",
            "Diesel Comum",
            "Diesel Aditivado",
            "Etanol Comum",
            "Etanol Aditivado"
        };

        public static void ValidarTipo(string tipo)
        {
            if (!tiposValidos.Contains(tipo))
            {
                throw new TipoCombustivelInvalidoException("Tipo de combustível inválido.");
            }
        }
    }
}
