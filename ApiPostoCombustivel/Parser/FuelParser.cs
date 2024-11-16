using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO.CombustivelDTO;

namespace ApiPostoCombustivel.Parser
{
    public class FuelParser
    {
        // Converte um objeto FuelDTO em um modelo de banco de dados (TbFuel).
        public static TbFuel ToModel(FuelDTO dto)
        {
            return new TbFuel
            {
                // Mapeia o ID do DTO para o modelo.
                Id = dto.Id,
                // Mapeia o tipo de combustível do DTO para o modelo.
                Type = dto.Type,
                // Mapeia o estoque do DTO para o modelo.
                Stock = dto.Stock
            };
        }

        // Converte um modelo de banco de dados (TbFuel) em um objeto FuelDTO.
        public static FuelDTO ToDTO(TbFuel model)
        {
            return new FuelDTO
            {
                // Mapeia o ID do modelo para o DTO.
                Id = model.Id,
                // Mapeia o tipo de combustível do modelo para o DTO.
                Type = model.Type,
                // Mapeia o estoque do modelo para o DTO.
                Stock = model.Stock
            };
        }
    }
}
