using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Validations;
using ApiPostoCombustivel.DTO.PrecoDTO;

namespace ApiPostoCombustivel.Services
{
    public class PrecoService
    {
        private readonly PrecoRepository _precoRepository;
        private readonly CombustivelRepository _combustivelRepository;

        public PrecoService(AppDbContext context)
        {
            _precoRepository = new PrecoRepository(context);
            _combustivelRepository = new CombustivelRepository(context);
        }

        public IEnumerable<PrecoDTO> GetPrecos()
        {
            var precos = _precoRepository.GetPrecos();
            return precos.Select(PrecoParser.ToDTO);
        }

        public PrecoDTO GetPrecoById(int id)
        {
            PrecoExistenciaValidator.ValidarPrecoExistente(_precoRepository, id);

            var preco = _precoRepository.GetPrecoById(id);
            return PrecoParser.ToDTO(preco);
        }

        public PrecoDTO GetPrecoByCombustivelId(int combustivelId)
        {
            CombustivelExistenciaValidator.ValidarCombustivelExistente(_combustivelRepository, combustivelId);

            var preco = _precoRepository.GetPrecoByCombustivelId(combustivelId);
            return preco != null ? PrecoParser.ToDTO(preco) : null;
        }

        public PrecoDTO AddPreco(PrecoDTO precoDto)
        {
            CombustivelExistenciaValidator.ValidarCombustivelExistente(_combustivelRepository, precoDto.CombustivelId);

            PrecoValidator.ValidarCriacaoPreco(precoDto.PrecoPorLitro, precoDto.DataInicio);

            if (precoDto.DataFim != null)
            {
                PrecoValidator.ValidarPeriodo(precoDto.DataInicio, precoDto.DataFim.Value);
            }

            PrecoExistenciaValidator.ValidarDataFimParaNovoPreco(_precoRepository, precoDto.CombustivelId);

            PrecoExistenciaValidator.ValidarDuplicidadePreco(_precoRepository, precoDto.CombustivelId, precoDto.DataInicio, precoDto.DataFim);

            var preco = PrecoParser.ToModel(precoDto);
            _precoRepository.AddPreco(preco);
            return PrecoParser.ToDTO(preco);
        }

        public void UpdatePreco(int id, UpdatePrecoDTO updateDto)
        {
            PrecoExistenciaValidator.ValidarPrecoExistente(_precoRepository, id);

            var preco = _precoRepository.GetPrecoById(id);

            PrecoValidator.ValidarEdicaoPreco(updateDto.PrecoPorLitro, updateDto.DataInicio, updateDto.DataFim);


            if (updateDto.DataFim == null)
            {
                PrecoExistenciaValidator.ValidarDataFimParaAtualizacao(_precoRepository, preco.CombustivelId, id);
            }


            if (updateDto.DataInicio.HasValue || updateDto.DataFim.HasValue)
            {
                PrecoExistenciaValidator.ValidarDuplicidadePreco(
                    _precoRepository,
                    preco.CombustivelId,
                    updateDto.DataInicio ?? preco.DataInicio,
                    updateDto.DataFim ?? preco.DataFim,
                    id
                );
            }

            if (updateDto.PrecoPorLitro.HasValue)
            {
                preco.PrecoPorLitro = updateDto.PrecoPorLitro.Value;
            }

            if (updateDto.DataInicio.HasValue)
            {
                preco.DataInicio = updateDto.DataInicio.Value;
            }

            if (updateDto.DataFim.HasValue)
            {
                preco.DataFim = updateDto.DataFim.Value;
            }
            else if (updateDto.DataFim == null && updateDto.DataFim is not null)
            {
                preco.DataFim = null; 
            }

            _precoRepository.UpdatePreco(preco);
        }


        public void DeletePreco(int id)
        {
            PrecoExistenciaValidator.ValidarPrecoExistente(_precoRepository, id);

            _precoRepository.DeletePreco(id);
        }
    }
}
