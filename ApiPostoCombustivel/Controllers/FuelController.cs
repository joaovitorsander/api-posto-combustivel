using ApiPostoCombustivel.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.DTO.CombustivelDTO;
using ApiPostoCombustivel.Exceptions;

namespace ApiPostoCombustivel.Controllers
{
    [Route("api/fuels")]
    [ApiController]
    public class FuelController : ControllerBase
    {
        private readonly FuelService _service;
        private readonly ILogger<FuelController> _logger;

        public FuelController(AppDbContext context, ILogger<FuelController> logger)
        {
            _service = new FuelService(context);
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FuelDTO>> GetInventory()
        {
            var stock = _service.GetInventory();
            return Ok(stock);
        }

        [HttpGet("{id:int}")]
        public ActionResult<FuelDTO> GetFuelById(int id)
        {
            try
            {
                var fuel = _service.GetFuelById(id);
                return Ok(fuel);
            }
            catch (FuelNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpGet("tipo/{type}")]
        public ActionResult<FuelDTO> GetFuelByType(string type)
        {
            try
            {
                var fuel = _service.GetFuelByType(type);
                return Ok(fuel);
            }
            catch (InvalidFuelTypeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FuelNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpPost]
        public IActionResult AddFuel([FromBody] CreateFuelDTO createDto)
        {
            _logger.LogInformation("Iniciando o processo de Adicionar Combustível.");

            try
            {
                _logger.LogInformation("Tentando adicionar um novo tipo de combustível: {FuelType} com estoque: {Stock}.", createDto.Type, createDto.Stock);

                var fuelDto = new FuelDTO
                {
                    Type = createDto.Type,
                    Stock = createDto.Stock
                };

                var result = _service.AddFuel(fuelDto);

                _logger.LogInformation("Tipo de combustível adicionado com sucesso: {FuelType} com ID: {FuelId}.", result.Type, result.Id);

                return CreatedAtAction(nameof(GetFuelById), new { id = result.Id }, result);
            }
            catch (FuelAlreadyRegisteredException ex)
            {
                _logger.LogWarning("Tipo de combustível já registrado: {FuelType}. Exceção: {ExceptionMessage}", createDto.Type, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidFuelTypeException ex)
            {
                _logger.LogWarning("Tipo de combustível inválido: {FuelType}. Exceção: {ExceptionMessage}", createDto.Type, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidStockException ex)
            {
                _logger.LogWarning("Valor de estoque inválido para o tipo de combustível: {FuelType}. Exceção: {ExceptionMessage}", createDto.Type, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Erro inesperado ao adicionar o tipo de combustível: {FuelType}. Exceção: {ExceptionMessage}", createDto.Type, e.Message);
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
            finally
            {
                _logger.LogInformation("Processo de Adicionar Combustível finalizado.");
            }
        }


        [HttpPatch("{id:int}")]
        public IActionResult UpdateFuel(int id, [FromBody] UpdateFuelDTO updateDto)
        {
            _logger.LogInformation("Iniciando o processo de Atualização do Combustível com ID: {FuelId}.", id);

            try
            {
                _logger.LogInformation("Atualizando o combustível com ID: {FuelId}. Dados recebidos: Tipo: {FuelType}, Estoque: {Stock}.",
                    id, updateDto.Type, updateDto.Stock);

                _service.UpdateFuel(id, updateDto);

                _logger.LogInformation("Combustível com ID: {FuelId} atualizado com sucesso.", id);

                return NoContent();
            }
            catch (FuelNotFoundException ex)
            {
                _logger.LogWarning("Combustível não encontrado para o ID: {FuelId}. Exceção: {ExceptionMessage}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidFuelTypeException ex)
            {
                _logger.LogWarning("Tipo de combustível inválido para o ID: {FuelId}. Tipo informado: {FuelType}. Exceção: {ExceptionMessage}",
                    id, updateDto.Type, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidStockException ex)
            {
                _logger.LogWarning("Valor de estoque inválido para o ID: {FuelId}. Estoque informado: {Stock}. Exceção: {ExceptionMessage}",
                    id, updateDto.Stock, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Erro inesperado ao atualizar o combustível com ID: {FuelId}. Exceção: {ExceptionMessage}", id, e.Message);
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
            finally
            {
                _logger.LogInformation("Processo de Atualização do Combustível finalizado para o ID: {FuelId}.", id);
            }
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteFuel(int id)
        {
            try
            {
                _service.DeleteFuel(id);
                return NoContent();
            }
            catch (FuelNotFoundException ex)
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
