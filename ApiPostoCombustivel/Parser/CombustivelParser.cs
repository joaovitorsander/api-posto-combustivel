using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO;

namespace ApiPostoCombustivel.Parser
{
    public class CombustivelParser
    {
        public static TbCombustivel ToModel(CombustivelDTO dto)
        {
            return new TbCombustivel
            {
                Tipo = dto.Tipo,
                Quantidade = dto.Quantidade
            };
        }

        public static CombustivelDTO ToDTO(TbCombustivel model)
        {
            return new CombustivelDTO
            {
                Tipo = model.Tipo,
                Quantidade = model.Quantidade
            };
        }
    }
}
