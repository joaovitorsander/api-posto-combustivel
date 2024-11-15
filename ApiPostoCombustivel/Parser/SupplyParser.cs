using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO;
using ApiPostoCombustivel.DTO.AbastecimentoDTO;

namespace ApiPostoCombustivel.Parser
{
    public class SupplyParser
    {
        public static TbSupply ToModel(SupplyDTO dto)
        {
            return new TbSupply
            {
                Id = dto.Id,
                FuelType = dto.FuelType,
                Quantity = dto.Quantity,
                Date = dto.Date,
                TotalValue = dto.Value
            };
        }

        public static SupplyDTO ToDTO(TbSupply model)
        {
            return new SupplyDTO
            {
                Id = model.Id,
                FuelType = model.FuelType,
                Quantity = model.Quantity,
                Date = model.Date,
                Value = model.TotalValue
            };
        }
    }
}
