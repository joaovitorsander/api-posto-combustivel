using ApiPostoCombustivel.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.DTO.CombustivelDTO;

namespace ApiPostoCombustivel.Controllers
{
    [Route("api/combustivel")]
    [ApiController]
    public class CombustivelController : ControllerBase
    {
        private readonly CombustivelService _service;

        public CombustivelController(AppDbContext context)
        {
            _service = new CombustivelService(context);
        }

        [HttpGet]
        public ActionResult<IEnumerable<CombustivelDTO>> GetEstoque()
        {
            return Ok(_service.GetEstoque());
        }

        [HttpGet("{id:int}")]
        public ActionResult<CombustivelDTO> GetCombustivelById(int id)
        {
            var combustivel = _service.GetCombustivelById(id);
            if (combustivel == null)
                return NotFound();
            return Ok(combustivel);
        }

        [HttpGet("tipo/{tipo}")]
        public ActionResult<CombustivelDTO> GetCombustivelByTipo(string tipo)
        {
            var combustivel = _service.GetCombustivelByTipo(tipo);
            if (combustivel == null)
                return NotFound();
            return Ok(combustivel);
        }

        [HttpPost]
        public IActionResult AddCombustivel([FromBody] CreateCombustivelDTO createDto)
        {
            try
            {
                var combustivelDto = new CombustivelDTO
                {
                    Tipo = createDto.Tipo,
                    Estoque = createDto.Estoque
                };

                var resultado = _service.AddCombustivel(combustivelDto);
                return CreatedAtAction(nameof(GetCombustivelById), new { id = resultado.Id }, resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateCombustivel(int id, [FromBody] UpdateCombustivelDTO updateDto)
        {
            var existingCombustivel = _service.GetCombustivelById(id);
            if (existingCombustivel == null)
                return NotFound();

            try
            {
                _service.UpdateCombustivel(id, updateDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCombustivel(int id)
        {
            var combustivel = _service.GetCombustivelById(id);
            if (combustivel == null)
                return NotFound();

            _service.DeleteCombustivel(id);
            return NoContent();
        }
    }
}
