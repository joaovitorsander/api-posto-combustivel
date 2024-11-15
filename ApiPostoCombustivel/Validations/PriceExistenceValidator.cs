using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Validations
{
    public class PriceExistenceValidator
    {
        public static void ValidatePriceExistence(PriceRepository priceRepository, int id)
        {
            var price = priceRepository.GetPriceById(id);
            if (price == null)
            {
                throw new PriceNotFoundException("Preço não encontrado.");
            }
        }

        public static void ValidateDuplicatePrice(PriceRepository priceRepository, int fuelId, DateTime startDate, DateTime? endDate, int? currentPriceId = null)
        {
            var prices = priceRepository.GetPrices()
                .Where(p => p.FuelId == fuelId);

            if (currentPriceId.HasValue)
            {
                prices = prices.Where(p => p.Id != currentPriceId.Value);
            }

            var isDuplicate = prices.Any(p =>
                (endDate.HasValue && p.EndDate.HasValue && p.StartDate <= endDate && p.EndDate >= startDate) || 
                (!p.EndDate.HasValue && p.StartDate <= startDate) ||
                (endDate == null && p.EndDate >= startDate));

            if (isDuplicate)
            {
                throw new DuplicatePriceException("Já existe um preço registrado que abrange o mesmo período ou datas conflitantes.");
            }
        }

        public static void ValidateEndDateForNewPrice(PriceRepository priceRepository, int fuelId)
        {
            var lastPrice = priceRepository.GetPrices()
                .Where(p => p.FuelId == fuelId)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            if (lastPrice != null && !lastPrice.EndDate.HasValue)
            {
                throw new UndefinedEndDateException("Não é possível adicionar um novo preço sem que o preço anterior tenha uma DataFim definida.");
            }
        }

        public static void ValidateEndDateForUpdate(PriceRepository priceRepository, int fuelId, int priceId)
        {
            var futurePrices = priceRepository.GetPrices()
                .Where(p => p.FuelId == fuelId && p.Id != priceId && p.StartDate > DateTime.UtcNow)
                .ToList();

            if (futurePrices.Any())
            {
                throw new UndefinedEndDateException("Não é possível definir a DataFim como null enquanto existir um preço futuro registrado.");
            }
        }
    }
}
