using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class PriceExistenceValidator
    {
        // Método para validar se o preço com o ID fornecido existe no repositório.
        public static void ValidatePriceExistence(PriceRepository priceRepository, int id)
        {
            // Recupera o preço pelo ID no repositório.
            var price = priceRepository.GetPriceById(id);
            // Se o preço não for encontrado, lança uma exceção.
            if (price == null)
            {
                throw new PriceNotFoundException("Preço não encontrado.");
            }
        }

        // Método para validar se já existe um preço duplicado para o mesmo combustível e período.
        // Isso inclui o caso de um preço já registrado para o mesmo combustível em datas conflitantes.
        public static void ValidateDuplicatePrice(PriceRepository priceRepository, int fuelId, DateTime startDate, DateTime? endDate, int? currentPriceId = null)
        {
            // Recupera todos os preços para o combustível informado.
            var prices = priceRepository.GetPrices()
                .Where(p => p.FuelId == fuelId);

            // Se um ID de preço atual for fornecido, exclui esse preço da verificação.
            if (currentPriceId.HasValue)
            {
                prices = prices.Where(p => p.Id != currentPriceId.Value);
            }

            // Verifica se já existe algum preço que se sobrepõe ao novo preço (mesmo período).
            var isDuplicate = prices.Any(p =>
                (endDate.HasValue && p.EndDate.HasValue && p.StartDate <= endDate && p.EndDate >= startDate) || // Caso o preço existente tenha uma DataFim definida e sobreponha as datas.
                (!p.EndDate.HasValue && p.StartDate <= startDate) || // Caso o preço existente não tenha DataFim e abranja o início do novo preço.
                (endDate == null && p.EndDate >= startDate)); // Caso o preço existente não tenha DataFim e abranja o início do novo preço.

            // Se um preço duplicado for encontrado, lança uma exceção.
            if (isDuplicate)
            {
                throw new DuplicatePriceException("Já existe um preço registrado que abrange o mesmo período ou datas conflitantes.");
            }
        }

        // Método para validar se um preço anterior possui uma DataFim definida antes de adicionar um novo preço.
        public static void ValidateEndDateForNewPrice(PriceRepository priceRepository, int fuelId)
        {
            // Recupera o último preço registrado para o combustível informado, ordenado pela data de início.
            var lastPrice = priceRepository.GetPrices()
                .Where(p => p.FuelId == fuelId)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            // Se o último preço não possui DataFim, lança uma exceção indicando que o preço anterior precisa ter uma DataFim definida.
            if (lastPrice != null && !lastPrice.EndDate.HasValue)
            {
                throw new UndefinedEndDateException("Não é possível adicionar um novo preço sem que o preço anterior tenha uma DataFim definida.");
            }
        }

        // Método para validar se a DataFim de um preço pode ser atualizada para null, verificando se há preços futuros.
        public static void ValidateEndDateForUpdate(PriceRepository priceRepository, int fuelId, int priceId)
        {
            // Recupera todos os preços futuros para o combustível informado, excluindo o preço atual.
            var futurePrices = priceRepository.GetPrices()
                .Where(p => p.FuelId == fuelId && p.Id != priceId && p.StartDate > DateTime.UtcNow)
                .ToList();

            // Se houver preços futuros registrados, lança uma exceção indicando que a DataFim não pode ser null.
            if (futurePrices.Any())
            {
                throw new UndefinedEndDateException("Não é possível definir a DataFim como null enquanto existir um preço futuro registrado.");
            }
        }
    }
}
