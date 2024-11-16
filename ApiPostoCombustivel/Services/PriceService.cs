using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Validations;
using ApiPostoCombustivel.DTO.PrecoDTO;

namespace ApiPostoCombustivel.Services
{
    public class PriceService
    {
        // Repositórios para interagir com os dados de preços e combustíveis no banco de dados.
        private readonly PriceRepository _priceRepository;
        private readonly FuelRepository _fuelRepository;

        // Construtor que inicializa os repositórios de preço e combustível, usando o contexto do banco de dados.
        public PriceService(AppDbContext context)
        {
            _priceRepository = new PriceRepository(context);
            _fuelRepository = new FuelRepository(context);
        }

        // Retorna uma lista de preços de combustíveis, convertendo os modelos de banco para DTOs.
        public IEnumerable<PriceDTO> GetPrices()
        {
            var prices = _priceRepository.GetPrices(); // Obtém os preços de combustíveis do repositório.
            return prices.Select(PriceParser.ToDTO); // Converte os modelos para DTOs e retorna.
        }

        // Retorna o preço de combustível com base no ID fornecido.
        // Antes de buscar o preço, valida se ele existe.
        public PriceDTO GetPriceById(int id)
        {
            // Valida se o preço com o ID fornecido existe no banco de dados.
            PriceExistenceValidator.ValidatePriceExistence(_priceRepository, id);

            // Obtém o preço do banco de dados e converte para DTO.
            var price = _priceRepository.GetPriceById(id);
            return PriceParser.ToDTO(price);
        }

        // Retorna o preço do combustível com base no ID do combustível fornecido.
        // Valida se o combustível existe antes de buscar o preço.
        public PriceDTO GetPriceByFuelId(int fuelId)
        {
            // Valida se o combustível com o ID fornecido existe.
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, fuelId);

            // Obtém o preço do combustível com base no ID do combustível.
            var price = _priceRepository.GetPriceByFuelId(fuelId);

            // Se o preço existir, converte para DTO e retorna, caso contrário retorna null.
            return price != null ? PriceParser.ToDTO(price) : null;
        }

        // Adiciona um novo preço de combustível.
        // Realiza várias validações antes de adicionar o preço ao banco de dados.
        public PriceDTO AddPrice(PriceDTO priceDto)
        {
            // Valida se o combustível existe no banco de dados com o ID fornecido.
            FuelExistenceValidator.ValidateFuelExistence(_fuelRepository, priceDto.FuelId);

            // Valida o preço e a data de início do novo preço.
            PriceValidator.ValidatePriceCreation(priceDto.PricePerLiter, priceDto.StartDate);

            // Se a data de término for fornecida, valida o período.
            if (priceDto.EndDate != null)
            {
                PriceValidator.ValidatePeriod(priceDto.StartDate, priceDto.EndDate.Value);
            }

            // Valida se já existe um preço registrado para o combustível no período.
            PriceExistenceValidator.ValidateEndDateForNewPrice(_priceRepository, priceDto.FuelId);

            // Valida se o preço para o combustível no período fornecido já não está registrado.
            PriceExistenceValidator.ValidateDuplicatePrice(_priceRepository, priceDto.FuelId, priceDto.StartDate, priceDto.EndDate);

            // Converte o DTO para o modelo de banco de dados e adiciona ao repositório.
            var price = PriceParser.ToModel(priceDto);
            _priceRepository.AddPrice(price);

            // Converte o modelo salvo de volta para DTO e retorna.
            return PriceParser.ToDTO(price);
        }

        // Atualiza os dados de um preço de combustível existente.
        // Valida a existência do preço e realiza verificações para evitar sobreposição de preços.
        public void UpdatePrice(int id, UpdatePriceDTO updateDto)
        {
            // Valida se o preço com o ID fornecido existe no banco de dados.
            PriceExistenceValidator.ValidatePriceExistence(_priceRepository, id);

            // Obtém o preço existente no banco de dados.
            var price = _priceRepository.GetPriceById(id);

            // Valida as alterações no preço, incluindo a data de início e de término.
            PriceValidator.ValidatePriceEdit(updateDto.PricePerLiter, updateDto.StartDate, updateDto.EndDate);

            // Se a data de término não for fornecida, valida a possibilidade de atualização sem ela.
            if (updateDto.EndDate == null)
            {
                PriceExistenceValidator.ValidateEndDateForUpdate(_priceRepository, price.FuelId, id);
            }

            // Se as datas de início ou término forem alteradas, valida a duplicação de preços.
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

            // Atualiza o preço, a data de início e a data de término conforme fornecido.
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

            // Salva as atualizações no banco de dados.
            _priceRepository.UpdatePrice(price);
        }

        // Remove um preço de combustível do banco de dados com base no ID fornecido.
        // Valida a existência do preço antes de deletá-lo.
        public void DeletePrice(int id)
        {
            // Valida se o preço com o ID fornecido existe no banco de dados.
            PriceExistenceValidator.ValidatePriceExistence(_priceRepository, id);

            // Exclui o preço do banco de dados.
            _priceRepository.DeletePrice(id);
        }
    }
}
