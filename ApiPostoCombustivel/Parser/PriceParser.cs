using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO.PrecoDTO;

namespace ApiPostoCombustivel.Parser
{
    public class PriceParser
    {
        // Converte um objeto PriceDTO em um modelo de banco de dados (TbPrice).
        public static TbPrice ToModel(PriceDTO dto)
        {
            return new TbPrice
            {
                // Mapeia o ID do DTO para o modelo.
                Id = dto.Id,
                // Mapeia o ID do combustível do DTO para o modelo.
                FuelId = dto.FuelId,
                // Mapeia o preço por litro do DTO para o modelo.
                PricePerLiter = dto.PricePerLiter,
                // Mapeia a data de início do DTO para o modelo.
                StartDate = dto.StartDate,
                // Mapeia a data de término do DTO para o modelo.
                EndDate = dto.EndDate
            };
        }

        // Converte um modelo de banco de dados (TbPrice) em um objeto PriceDTO.
        public static PriceDTO ToDTO(TbPrice model)
        {
            return new PriceDTO
            {
                // Mapeia o ID do modelo para o DTO.
                Id = model.Id,
                // Mapeia o ID do combustível do modelo para o DTO.
                FuelId = model.FuelId,
                // Mapeia o preço por litro do modelo para o DTO.
                PricePerLiter = model.PricePerLiter,
                // Mapeia a data de início do modelo para o DTO.
                StartDate = model.StartDate,
                // Mapeia a data de término do modelo para o DTO.
                EndDate = model.EndDate
            };
        }
    }
}
