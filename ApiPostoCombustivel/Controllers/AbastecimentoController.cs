using ApiPostoCombustivel.Services;
using ApiPostoCombustivel.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Repositories;
using ApiPostoCombustivel.Parser;

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
            var abastecimentoRepository = new AbastecimentoRepository(context);
            _service = new AbastecimentoService(abastecimentoRepository);

            var combustivelRepository = new CombustivelRepository(context);
            _combustivelService = new CombustivelService(combustivelRepository);
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

        // GET: api/abastecimento/relatorio/{data}
        [HttpGet("relatorio/{data}")]
        public IActionResult GetRelatorioPorDia(DateTime data)
        {

            var abastecimentosDoDia = _service.GetAbastecimentos()
                                              .Where(a => a.Data.Date == data.Date);

            var estoqueAtual = _combustivelService.GetEstoque();

            return Ok(new
            {
                AbastecimentosDiarios = abastecimentosDoDia,
                EstoqueAtual = estoqueAtual
            });
        }
    }
}
