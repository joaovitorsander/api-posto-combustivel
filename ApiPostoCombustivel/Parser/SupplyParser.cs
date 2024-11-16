using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO;
using ApiPostoCombustivel.DTO.AbastecimentoDTO;

namespace ApiPostoCombustivel.Parser
{
    public class SupplyParser
    {
        // Converte um objeto SupplyDTO em um modelo de banco de dados (TbSupply).
        public static TbSupply ToModel(SupplyDTO dto)
        {
            return new TbSupply
            {
                // Mapeia o ID do DTO para o modelo.
                Id = dto.Id,
                // Mapeia o tipo de combustível do DTO para o modelo.
                FuelType = dto.FuelType,
                // Mapeia a quantidade abastecida do DTO para o modelo.
                Quantity = dto.Quantity,
                // Mapeia a data do abastecimento do DTO para o modelo.
                Date = dto.Date,
                // Mapeia o valor total do abastecimento do DTO para o modelo.
                TotalValue = dto.Value
            };
        }

        // Converte um modelo de banco de dados (TbSupply) em um objeto SupplyDTO.
        public static SupplyDTO ToDTO(TbSupply model)
        {
            return new SupplyDTO
            {
                // Mapeia o ID do modelo para o DTO.
                Id = model.Id,
                // Mapeia o tipo de combustível do modelo para o DTO.
                FuelType = model.FuelType,
                // Mapeia a quantidade abastecida do modelo para o DTO.
                Quantity = model.Quantity,
                // Mapeia a data do abastecimento do modelo para o DTO.
                Date = model.Date,
                // Mapeia o valor total do abastecimento do modelo para o DTO.
                Value = model.TotalValue
            };
        }
    }
}
