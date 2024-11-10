using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO;

namespace ApiPostoCombustivel.Parser
{
    public class AbastecimentoParser
    {
        public static TbAbastecimento ToModel(AbastecimentoDTO dto)
        {
            return new TbAbastecimento
            {
                TipoCombustivel = dto.TipoCombustivel,
                Quantidade = dto.Quantidade,
                Data = dto.Data 
            };
        }

        public static AbastecimentoDTO ToDTO(TbAbastecimento model)
        {
            return new AbastecimentoDTO
            {
                TipoCombustivel = model.TipoCombustivel,
                Quantidade = model.Quantidade,
                Data = model.Data 
            };
        }
    }
}
