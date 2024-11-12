using ApiPostoCombustivel.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Repositories;
using ApiPostoCombustivel.DTO.AbastecimentoDTO;

namespace ApiPostoCombustivel.Controllers
{
    [Route("api/abastecimento")]
    [ApiController]
    public class AbastecimentoController : ControllerBase
    {
        private readonly AbastecimentoService _service;
        private readonly CombustivelRepository _combustivelRepository;

        public AbastecimentoController(AppDbContext context)
        {
            var abastecimentoRepository = new AbastecimentoRepository(context);
            _combustivelRepository = new CombustivelRepository(context); 

            _service = new AbastecimentoService(abastecimentoRepository, _combustivelRepository);
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
            catch (InvalidOperationException ex)
            {
                return BadRequest("Estoque insuficiente para realizar a atualização do abastecimento.");
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
                                              .Where(a => a.Data.Date == data.Date);

            var estoqueAtual = _combustivelRepository.GetEstoque();

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
