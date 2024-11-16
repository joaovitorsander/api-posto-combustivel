using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class FuelExistenceValidator
    {
        // Método que valida se o combustível já está registrado no repositório.
        // Se o combustível já estiver registrado, lança uma exceção.
        public static void ValidateFuelAlreadyRegistered(FuelRepository fuelRepository, string type)
        {
            // Verifica se já existe um combustível com o mesmo tipo no repositório.
            var existingFuel = fuelRepository.GetFuelByType(type);

            // Caso o combustível exista, uma exceção é lançada para impedir o registro duplicado.
            if (existingFuel != null)
            {
                throw new FuelAlreadyRegisteredException("Este tipo de combustível já está registrado.");
            }
        }

        // Método que valida a existência de um combustível no repositório a partir do ID.
        // Se o combustível não for encontrado, lança uma exceção.
        public static void ValidateFuelExistence(FuelRepository fuelRepository, int id)
        {
            // Busca o combustível no repositório pelo ID.
            var fuel = fuelRepository.GetFuelById(id);

            // Se não encontrar o combustível, lança uma exceção informando que o combustível não foi encontrado.
            if (fuel == null)
            {
                throw new FuelNotFoundException("Combustível não encontrado.");
            }
        }
    }
}
