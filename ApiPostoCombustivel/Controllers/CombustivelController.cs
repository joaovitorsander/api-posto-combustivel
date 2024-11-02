using ApiPostoCombustivel.Services;
using ApiPostoCombustivel.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Repositories;

namespace ApiPostoCombustivel.Controllers
{
    [Route("api/combustivel")]
    [ApiController]
    public class CombustivelController : ControllerBase
    {
        private readonly CombustivelService _service;

        public CombustivelController(AppDbContext context)
        {
            var repository = new CombustivelRepository(context);
            _service = new CombustivelService(repository);
        }

        // GET: api/combustivel
        [HttpGet]
        public ActionResult<IEnumerable<CombustivelDTO>> GetEstoque()
        {
            return Ok(_service.GetEstoque());
        }

        // GET: api/combustivel/{tipo}
        [HttpGet("{tipo}")]
        public ActionResult<CombustivelDTO> GetCombustivelByTipo(string tipo)
        {
            var combustivel = _service.GetCombustivelByTipo(tipo);
            if (combustivel == null)
                return NotFound();
            return Ok(combustivel);
        }

        // POST: api/combustivel
        [HttpPost]
        public IActionResult AddCombustivel([FromBody] CombustivelDTO combustivelDto)
        {
            _service.AddCombustivel(combustivelDto);
            return CreatedAtAction(nameof(GetCombustivelByTipo), new { tipo = combustivelDto.Tipo }, combustivelDto);
        }

        // PATCH: api/combustivel/{tipo}
        [HttpPatch("{tipo}")]
        public IActionResult UpdateCombustivel(string tipo, [FromBody] CombustivelDTO combustivelDto)
        {
            if (tipo != combustivelDto.Tipo)
                return BadRequest("Tipo de combustível não corresponde.");

            var existingCombustivel = _service.GetCombustivelByTipo(tipo);
            if (existingCombustivel == null)
                return NotFound();

            _service.UpdateCombustivel(combustivelDto);
            return NoContent();
        }

        // DELETE: api/combustivel/{tipo}
        [HttpDelete("{tipo}")]
        public IActionResult DeleteCombustivel(string tipo)
        {
            var combustivel = _service.GetCombustivelByTipo(tipo);
            if (combustivel == null)
                return NotFound();

            _service.DeleteCombustivel(tipo);
            return NoContent();
        }
    }
}
