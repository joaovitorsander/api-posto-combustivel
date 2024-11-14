using ApiPostoCombustivel.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.DTO.AbastecimentoDTO;
using ApiPostoCombustivel.Database.Repositories;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Controllers
{
    [Route("api/abastecimento")]
    [ApiController]
    public class AbastecimentoController : ControllerBase
    {
        private readonly AbastecimentoService _service;
        private readonly CombustivelService _combustivelService;

        public AbastecimentoController(AppDbContext context)
        {
            _service = new AbastecimentoService(context);
            _combustivelService = new CombustivelService(context);
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
            try
            {
                var abastecimento = _service.GetAbastecimentoById(id);
                return Ok(abastecimento);
            }
            catch (AbastecimentoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

        // POST: api/abastecimento
        [HttpPost]
        public IActionResult AddAbastecimento([FromBody] CreateAbastecimentoDTO createDto)
        {
            try
            {
                var abastecimentoDto = new AbastecimentoDTO
                {
                    TipoCombustivel = createDto.TipoCombustivel,
                    Quantidade = createDto.Quantidade,
                    Data = createDto.Data
                };

                var resultado = _service.AddAbastecimento(abastecimentoDto);
                return CreatedAtAction(nameof(GetAbastecimentoById), new { id = resultado.Id }, resultado);
            }
            catch (CombustivelNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (QuantidadeAbastecimentoInvalidaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EstoqueInsuficienteException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }


        // PATCH: api/abastecimento/{id}
        [HttpPatch("{id}")]
        public IActionResult UpdateAbastecimento(int id, [FromBody] UpdateAbastecimentoDTO updateDto)
        {
            try
            {
                var resultado = _service.UpdateAbastecimento(id, updateDto);
                return Ok(resultado);
            }
            catch (AbastecimentoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (TipoCombustivelInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CombustivelNaoEncontradoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EstoqueInsuficienteException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (QuantidadeAbastecimentoInvalidaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

        // DELETE: api/abastecimento/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAbastecimento(int id)
        {
            try
            {
                _service.DeleteAbastecimento(id);
                return NoContent();
            }
            catch (AbastecimentoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

        // GET: api/abastecimento/relatorio/{data}
        [HttpGet("relatorio/{data}")]
        public IActionResult GetRelatorioPorDia(DateTime data)
        {
            try
            {
                var relatorio = _service.GetRelatorioPorDia(data);
                return Ok(relatorio);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpGet("tipo/{tipoCombustivel}")]
        public ActionResult<IEnumerable<AbastecimentoDTO>> GetAbastecimentosByTipo(string tipoCombustivel)
        {
            try
            {
                var abastecimentos = _service.GetAbastecimentosByTipo(tipoCombustivel);
                return Ok(abastecimentos);
            }
            catch (AbastecimentoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CombustivelNaoEncontradoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

    }
}
