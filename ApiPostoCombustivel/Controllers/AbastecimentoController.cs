using ApiPostoCombustivel.Services;
using ApiPostoCombustivel.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Repositories;

namespace ApiPostoCombustivel.Controllers
{
    [Route("api/abastecimento")]
    [ApiController]
    public class AbastecimentoController : ControllerBase
    {
        private readonly AbastecimentoService _service;

        public AbastecimentoController(AppDbContext context)
        {
            var repository = new AbastecimentoRepository(context);
            _service = new AbastecimentoService(repository);
        }

        // GET: api/abastecimento
        [HttpGet]
        public ActionResult<IEnumerable<AbastecimentoDTO>> GetAbastecimentos()
        {
            return Ok(_service.GetAbastecimentos());
        }

        // GET: api/abastecimento/{id}
        [HttpGet("{id}")]
        public ActionResult<AbastecimentoDTO> GetAbastecimentoById(int id)
        {
            var abastecimento = _service.GetAbastecimentoById(id);
            if (abastecimento == null)
                return NotFound();
            return Ok(abastecimento);
        }

        // POST: api/abastecimento
        [HttpPost]
        public IActionResult AddAbastecimento([FromBody] AbastecimentoDTO abastecimentoDto)
        {
            _service.AddAbastecimento(abastecimentoDto);
            return CreatedAtAction(nameof(GetAbastecimentoById), new { id = abastecimentoDto.TipoCombustivel }, abastecimentoDto);
        }

        // PATCH: api/abastecimento/{id}
        [HttpPatch("{id}")]
        public IActionResult UpdateAbastecimento(int id, [FromBody] AbastecimentoDTO abastecimentoDto)
        {
            var existingAbastecimento = _service.GetAbastecimentoById(id);
            if (existingAbastecimento == null)
                return NotFound();

            _service.UpdateAbastecimento(id, abastecimentoDto);
            return NoContent();
        }

        // DELETE: api/abastecimento/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAbastecimento(int id)
        {
            var abastecimento = _service.GetAbastecimentoById(id);
            if (abastecimento == null)
                return NotFound();

            _service.DeleteAbastecimento(id);
            return NoContent();
        }
    }
}
