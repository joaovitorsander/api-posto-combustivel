using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO.PrecoDTO;

namespace ApiPostoCombustivel.Parser
{
    public class PriceParser
    {
        public static TbPrice ToModel(PriceDTO dto)
        {
            return new TbPrice
            {
                Id = dto.Id,
                FuelId = dto.FuelId,
                PricePerLiter = dto.PricePerLiter,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
        }

        public static PriceDTO ToDTO(TbPrice model)
        {
            return new PriceDTO
            {
                Id = model.Id,
                FuelId = model.FuelId,
                PricePerLiter = model.PricePerLiter,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
        }
    }
}
