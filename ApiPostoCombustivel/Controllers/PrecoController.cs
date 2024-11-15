using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.DTO.PrecoDTO;
using ApiPostoCombustivel.Exceptions;
using ApiPostoCombustivel.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPostoCombustivel.Controllers
{
    [Route("api/preco")]
    [ApiController]
    public class PrecoController : ControllerBase
    {
        private readonly PrecoService _service;

        public PrecoController(AppDbContext context)
        {
            _service = new PrecoService(context);
        }

        [HttpGet]
        public ActionResult<IEnumerable<PrecoDTO>> GetPrecos()
        {
            var precos = _service.GetPrecos();
            return Ok(precos);
        }

        [HttpGet("{id:int}")]
        public ActionResult<PrecoDTO> GetPrecoById(int id)
        {
            try
            {
                var preco = _service.GetPrecoById(id);
                return Ok(preco);
            }
            catch (PrecoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpPost]
        public IActionResult AddPreco([FromBody] CreatePrecoDTO createDto)
        {
            try
            {
                var precoDto = new PrecoDTO
                {
                    PrecoPorLitro = createDto.PrecoPorLitro,
                    DataInicio = createDto.DataInicio,
                    DataFim = createDto.DataFim,
                    CombustivelId = createDto.CombustivelId
                };

                var resultado = _service.AddPreco(precoDto);
                return CreatedAtAction(nameof(GetPrecoById), new { id = resultado.Id }, resultado);
            }
            catch (PrecoDuplicadoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValorInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PeriodoInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CombustivelNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DataInicioObrigatoriaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DataFimNaoDefinidaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdatePreco(int id, [FromBody] UpdatePrecoDTO updateDto)
        {
            try
            {
                _service.UpdatePreco(id, updateDto);
                return NoContent();
            }
            catch (PrecoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValorInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PeriodoInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PrecoDuplicadoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePreco(int id)
        {
            try
            {
                _service.DeletePreco(id);
                return NoContent();
            }
            catch (PrecoNaoEncontradoException ex)
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
