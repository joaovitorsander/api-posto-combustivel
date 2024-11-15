using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Validations;
using ApiPostoCombustivel.DTO.PrecoDTO;

namespace ApiPostoCombustivel.Services
{
    public class PriceService
    {
        private readonly PriceRepository _priceRepository;
        private readonly FuelRepository _fuelRepository;

        public PriceService(AppDbContext context)
        {
            _priceRepository = new PriceRepository(context);
            _fuelRepository = new FuelRepository(context);
        }

        public IEnumerable<PriceDTO> GetPrices()
        {
            var prices = _priceRepository.GetPrices();
            return prices.Select(PriceParser.ToDTO);
        }

        public PriceDTO GetPriceById(int id)
        {
            PriceExistenceValidator.ValidatePriceExistence(_priceRepository, id);

            var price = _priceRepository.GetPriceById(id);
            return PriceParser.ToDTO(price);
        }

        public PriceDTO GetPriceByFuelId(int fuelId)
        {
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, fuelId);

            var price = _priceRepository.GetPriceByFuelId(fuelId);
            return price != null ? PriceParser.ToDTO(price) : null;
        }

        public PriceDTO AddPrice(PriceDTO priceDto)
        {
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, priceDto.FuelId);

            PriceValidator.ValidatePriceCreation(priceDto.PricePerLiter, priceDto.StartDate);

            if (priceDto.EndDate != null)
            {
                PriceValidator.ValidatePeriod(priceDto.StartDate, priceDto.EndDate.Value);
            }

            PriceExistenceValidator.ValidateEndDateForNewPrice(_priceRepository, priceDto.FuelId);

            PriceExistenceValidator.ValidateDuplicatePrice(_priceRepository, priceDto.FuelId, priceDto.StartDate, priceDto.EndDate);

            var price = PriceParser.ToModel(priceDto);
            _priceRepository.AddPrice(price);
            return PriceParser.ToDTO(price);
        }

        public void UpdatePrice(int id, UpdatePriceDTO updateDto)
        {
            PriceExistenceValidator.ValidatePriceExistence(_priceRepository, id);

            var price = _priceRepository.GetPriceById(id);

            PriceValidator.ValidatePriceEdit(updateDto.PricePerLiter, updateDto.StartDate, updateDto.EndDate);


            if (updateDto.EndDate == null)
            {
                PriceExistenceValidator.ValidateEndDateForUpdate(_priceRepository, price.FuelId, id);
            }


            if (updateDto.StartDate.HasValue || updateDto.EndDate.HasValue)
            {
                PriceExistenceValidator.ValidateDuplicatePrice(
                    _priceRepository,
                    price.FuelId,
                    updateDto.StartDate ?? price.StartDate,
                    updateDto.EndDate ?? price.EndDate,
                    id
                );
            }

            if (updateDto.PricePerLiter.HasValue)
            {
                price.PricePerLiter = updateDto.PricePerLiter.Value;
            }

            if (updateDto.StartDate.HasValue)
            {
                price.StartDate = updateDto.StartDate.Value;
            }

            if (updateDto.EndDate.HasValue)
            {
                price.EndDate = updateDto.EndDate.Value;
            }
            else if (updateDto.EndDate == null && updateDto.EndDate is not null)
            {
                price.EndDate = null; 
            }

            _priceRepository.UpdatePrice(price);
        }


        public void DeletePrice(int id)
        {
            PriceExistenceValidator.ValidatePriceExistence(_priceRepository, id);

            _priceRepository.DeletePrice(id);
        }
    }
}
