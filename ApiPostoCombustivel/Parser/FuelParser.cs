using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO.CombustivelDTO;

namespace ApiPostoCombustivel.Parser
{
    public class FuelParser
    {
        public static TbFuel ToModel(FuelDTO dto)
        {
            return new TbFuel
            {
                Id = dto.Id,
                Type = dto.Type,
                Stock = dto.Stock
            };
        }

        public static FuelDTO ToDTO(TbFuel model)
        {
            return new FuelDTO
            {
                Id = model.Id,
                Type = model.Type,
                Stock = model.Stock
            };
        }
    }
}
