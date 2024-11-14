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
            var estoque = _service.GetEstoque();
            return Ok(estoque);
        }

        [HttpGet("{id:int}")]
        public ActionResult<CombustivelDTO> GetCombustivelById(int id)
        {
            try
            {
                var combustivel = _service.GetCombustivelById(id);
                return Ok(combustivel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpGet("tipo/{tipo}")]
        public ActionResult<CombustivelDTO> GetCombustivelByTipo(string tipo)
        {
            try
            {
                var combustivel = _service.GetCombustivelByTipo(tipo);
                return Ok(combustivel);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateCombustivel(int id, [FromBody] UpdateCombustivelDTO updateDto)
        {
            try
            {
                _service.UpdateCombustivel(id, updateDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCombustivel(int id)
        {
            try
            {
                _service.DeleteCombustivel(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }
    }
}
