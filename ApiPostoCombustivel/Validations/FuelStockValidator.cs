using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class FuelStockValidator
    {
        public static void ValidateStock(double? stock)
        {
            if (stock <= 0)
            {
                throw new InvalidStockException("O estoque deve ser maior que zero.");
            }
        }

        public static void ValidateSufficientStock(double stock, double requiredQuantity)
        {
            if (stock < requiredQuantity)
            {
                throw new InsufficientStockException("Estoque insuficiente para realizar a operação.");
            }
        }
    }
}