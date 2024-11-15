using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class FuelExistenceValidator
    {
        public static void ValidateFuelAlreadyRegistered(FuelRepository fuelRepository, string type)
        {
            var existingFuel = fuelRepository.GetFuelByType(type);
            if (existingFuel != null)
            {
                throw new FuelAlreadyRegisteredException("Este tipo de combustível já está registrado.");
            }
        }

        public static void ValidateFuelExistence(FuelRepository fuelRepository, int id)
        {
            var fuel = fuelRepository.GetFuelById(id);
            if (fuel == null)
            {
                throw new FuelNotFoundException("Combustível não encontrado.");
            }
        }
    }
}
