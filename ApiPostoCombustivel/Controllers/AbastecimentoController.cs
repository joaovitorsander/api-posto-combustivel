using ApiPostoCombustivel.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.DTO.AbastecimentoDTO;
using ApiPostoCombustivel.Database.Repositories;

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
            var abastecimento = _service.GetAbastecimentoById(id);
            if (abastecimento == null)
                return NotFound();
            return Ok(abastecimento);
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Estoque insuficiente para realizar o abastecimento.");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
          //Criar pasta de exceptions 
            catch (InvalidOperationException ex)
            {
                return BadRequest("Estoque insuficiente para realizar a atualização do abastecimento.");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
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

        // GET: api/abastecimento/relatorio/{data}
        [HttpGet("relatorio/{data}")]
        public IActionResult GetRelatorioPorDia(DateTime data)
        {
            var abastecimentosDoDia = _service.GetAbastecimentos()
                                              .Where(a => a.Data.Date == data.Date)
                                              .ToList();

            var tiposCombustiveisAbastecidos = abastecimentosDoDia
                                                .Select(a => a.TipoCombustivel)
                                                .Distinct()
                                                .ToList();

            var estoqueAtual = _combustivelService.GetEstoque()
                                       .Where(c => tiposCombustiveisAbastecidos.Contains(c.Tipo))
                                       .ToList();

            return Ok(new
            {
                AbastecimentosDiarios = abastecimentosDoDia,
                EstoqueAtual = estoqueAtual
            });
        }

        [HttpGet("tipo/{tipoCombustivel}")]
        public ActionResult<IEnumerable<AbastecimentoDTO>> GetAbastecimentosByTipo(string tipoCombustivel)
        {
            var abastecimentos = _service.GetAbastecimentosByTipo(tipoCombustivel);
            if (!abastecimentos.Any())
                return NotFound();

            return Ok(abastecimentos);
        }

    }
}
