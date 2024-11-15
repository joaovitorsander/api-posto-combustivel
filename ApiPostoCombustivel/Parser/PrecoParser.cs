using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO.PrecoDTO;

namespace ApiPostoCombustivel.Parser
{
    public class PrecoParser
    {
        public static TbPreco ToModel(PrecoDTO dto)
        {
            return new TbPreco
            {
                Id = dto.Id,
                CombustivelId = dto.CombustivelId,
                PrecoPorLitro = dto.PrecoPorLitro,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim
            };
        }

        public static PrecoDTO ToDTO(TbPreco model)
        {
            return new PrecoDTO
            {
                Id = model.Id,
                CombustivelId = model.CombustivelId,
                PrecoPorLitro = model.PrecoPorLitro,
                DataInicio = model.DataInicio,
                DataFim = model.DataFim
            };
        }
    }
}
