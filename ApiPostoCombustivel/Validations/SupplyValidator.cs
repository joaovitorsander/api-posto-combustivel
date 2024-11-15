using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class SupplyValidator
    {
        public static void ValidateQuantity(double quantity)
        {
            if (quantity <= 0)
            {
                throw new InvalidSupplyQuantityException("A quantidade de abastecimento deve ser maior que zero.");
            }
        }

        public static void ValidateFuelExistence(FuelRepository fuelRepository, string fuelType)
        {
            var fuel = fuelRepository.GetFuelByType(fuelType);
            if (fuel == null)
            {
                throw new FuelNotFoundException("Tipo de combustível não encontrado. Cadastre o combustível antes de realizar o abastecimento.");
            }
        }

        public static void ValidateSufficientStock(double stock, double requiredQuantity)
        {
            if (stock < requiredQuantity)
            {
                throw new InsufficientStockException("Estoque insuficiente para realizar o abastecimento.");
            }
        }

        public static void ValidateSupplyExistence(SupplyRepository supplyRepository, int id)
        {
            var supply = supplyRepository.GetSupplyById(id);
            if (supply == null)
            {
                throw new SupplyNotFoundException("Abastecimento não encontrado.");
            }
        }
    }
}
