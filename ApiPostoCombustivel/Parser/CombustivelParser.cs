using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO.CombustivelDTO;

namespace ApiPostoCombustivel.Parser
{
    public class CombustivelParser
    {
        public static TbCombustivel ToModel(CombustivelDTO dto)
        {
            return new TbCombustivel
            {
                Id = dto.Id,
                Tipo = dto.Tipo,
                Estoque = dto.Estoque
            };
        }

        public static CombustivelDTO ToDTO(TbCombustivel model)
        {
            return new CombustivelDTO
            {
                Id = model.Id,
                Tipo = model.Tipo,
                Estoque = model.Estoque
            };
        }
    }
}
