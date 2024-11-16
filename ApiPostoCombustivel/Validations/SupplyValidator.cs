using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class SupplyValidator
    {
        // Valida se a quantidade de abastecimento é maior que zero.
        public static void ValidateQuantity(double quantity)
        {
            // Se a quantidade for menor ou igual a zero, lança uma exceção indicando que a quantidade deve ser maior que zero.
            if (quantity <= 0)
            {
                throw new InvalidSupplyQuantityException("A quantidade de abastecimento deve ser maior que zero.");
            }
        }

        // Valida se o combustível existe no repositório pelo tipo fornecido.
        // Caso o combustível não seja encontrado, lança uma exceção.
        public static void ValidateFuelExistence(FuelRepository fuelRepository, string fuelType)
        {
            // Verifica se o combustível com o tipo informado existe no repositório.
            var fuel = fuelRepository.GetFuelByType(fuelType);
            if (fuel == null)
            {
                // Se o combustível não existir, lança uma exceção indicando que o combustível não foi encontrado.
                throw new FuelNotFoundException("Tipo de combustível não encontrado. Cadastre o combustível antes de realizar o abastecimento.");
            }
        }

        // Valida se há estoque suficiente para realizar o abastecimento.
        public static void ValidateSufficientStock(double stock, double requiredQuantity)
        {
            // Se o estoque for menor que a quantidade requerida para o abastecimento, lança uma exceção indicando estoque insuficiente.
            if (stock < requiredQuantity)
            {
                throw new InsufficientStockException("Estoque insuficiente para realizar o abastecimento.");
            }
        }

        // Valida se o abastecimento existe no repositório pelo ID fornecido.
        public static void ValidateSupplyExistence(SupplyRepository supplyRepository, int id)
        {
            // Verifica se o abastecimento com o ID fornecido existe no repositório.
            var supply = supplyRepository.GetSupplyById(id);
            if (supply == null)
            {
                // Se o abastecimento não for encontrado, lança uma exceção indicando que o abastecimento não foi encontrado.
                throw new SupplyNotFoundException("Abastecimento não encontrado.");
            }
        }
    }
}
