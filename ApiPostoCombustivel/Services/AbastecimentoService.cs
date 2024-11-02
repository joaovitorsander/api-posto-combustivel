using ApiPostoCombustivel.DTO;
using ApiPostoCombustivel.Parser;
using ApiPostoCombustivel.Repositories;

namespace ApiPostoCombustivel.Services
{
    public class AbastecimentoService
    {
        private readonly IAbastecimentoRepository _repository;

        public AbastecimentoService(IAbastecimentoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<AbastecimentoDTO> GetAbastecimentos()
        {
            var abastecimentos = _repository.GetAbastecimentos();
            return abastecimentos.Select(AbastecimentoParser.ToDTO);
        }

        public AbastecimentoDTO GetAbastecimentoById(int id)
        {
            var abastecimento = _repository.GetAbastecimentoById(id);
            return abastecimento != null ? AbastecimentoParser.ToDTO(abastecimento) : null;
        }

        public void AddAbastecimento(AbastecimentoDTO abastecimentoDto)
        {
            var abastecimento = AbastecimentoParser.ToModel(abastecimentoDto);
            _repository.AddAbastecimento(abastecimento);
        }

        public void UpdateAbastecimento(int id, AbastecimentoDTO abastecimentoDto)
        {
            var abastecimento = AbastecimentoParser.ToModel(abastecimentoDto);
            abastecimento.Id = id;
            _repository.UpdateAbastecimento(abastecimento);
        }

        public void DeleteAbastecimento(int id)
        {
            _repository.DeleteAbastecimento(id);
        }
    }
}
