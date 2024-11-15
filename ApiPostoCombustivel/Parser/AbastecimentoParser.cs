using ApiPostoCombustivel.Database.Models;
using ApiPostoCombustivel.DTO;
using ApiPostoCombustivel.DTO.AbastecimentoDTO;

namespace ApiPostoCombustivel.Parser
{
    public class AbastecimentoParser
    {
        public static TbAbastecimento ToModel(AbastecimentoDTO dto)
        {
            return new TbAbastecimento
            {
                Id = dto.Id,
                TipoCombustivel = dto.TipoCombustivel,
                Quantidade = dto.Quantidade,
                Data = dto.Data,
                Valor = dto.Valor
            };
        }

        public static AbastecimentoDTO ToDTO(TbAbastecimento model)
        {
            return new AbastecimentoDTO
            {
                Id = model.Id,
                TipoCombustivel = model.TipoCombustivel,
                Quantidade = model.Quantidade,
                Data = model.Data,
                Valor = model.Valor
            };
        }
    }
}
