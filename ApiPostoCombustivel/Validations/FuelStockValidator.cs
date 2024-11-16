using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class FuelStockValidator
    {
        // Método que valida se o estoque do combustível é maior que zero.
        // Se o estoque for zero ou menor, uma exceção é lançada.
        public static void ValidateStock(double? stock)
        {
            // Verifica se o estoque informado é menor ou igual a zero.
            if (stock <= 0)
            {
                // Lança uma exceção indicando que o estoque não pode ser zero ou negativo.
                throw new InvalidStockException("O estoque deve ser maior que zero.");
            }
        }

        // Método que valida se há estoque suficiente para atender à quantidade necessária.
        // Se o estoque for insuficiente, uma exceção é lançada.
        public static void ValidateSufficientStock(double stock, double requiredQuantity)
        {
            // Verifica se o estoque atual é menor que a quantidade necessária.
            if (stock < requiredQuantity)
            {
                // Lança uma exceção indicando que o estoque é insuficiente para realizar a operação.
                throw new InsufficientStockException("Estoque insuficiente para realizar a operação.");
            }
        }
    }
}
