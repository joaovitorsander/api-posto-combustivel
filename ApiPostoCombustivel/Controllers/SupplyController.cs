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
    [Route("api/supplies")]
    [ApiController]
    public class SupplyController : ControllerBase
    {
        private readonly SupplyService _service;
        private readonly ILogger<SupplyController> _logger;
        public SupplyController(AppDbContext context, ILogger<SupplyController> logger)
        {
            _service = new SupplyService(context);
            _logger = logger;
        }


        [HttpGet]
        public ActionResult<IEnumerable<SupplyDTO>> GetSupplies()
        {
            return Ok(_service.GetSupplies());
        }

        [HttpGet("{id}")]
        public ActionResult<SupplyDTO> GetSupplyById(int id)
        {
            try
            {
                var supply = _service.GetSupplyById(id);
                return Ok(supply);
            }
            catch (SupplyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddSupply([FromBody] CreateSupplyDTO createDto)
        {
            _logger.LogInformation("Iniciando criação de abastecimento. Tipo de combustível: {FuelType}, Quantidade: {Quantity}, Data: {Date}.",
                createDto.FuelType, createDto.Quantity, createDto.Date);

            try
            {
                var supplyDto = new SupplyDTO
                {
                    FuelType = createDto.FuelType,
                    Quantity = createDto.Quantity,
                    Date = createDto.Date
                };

                var result = _service.AddSupply(supplyDto);

                _logger.LogInformation("Abastecimento criado com sucesso. ID: {SupplyId}, Valor Total: {TotalValue}.", result.Id, result.Value);

                return CreatedAtAction(nameof(GetSupplyById), new { id = result.Id }, result);
            }
            catch (FuelNotFoundException ex)
            {
                _logger.LogWarning("Erro ao criar abastecimento. Combustível não encontrado: {FuelType}. Exceção: {ExceptionMessage}", createDto.FuelType, ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidSupplyQuantityException ex)
            {
                _logger.LogWarning("Erro ao criar abastecimento. Quantidade inválida: {Quantity}. Exceção: {ExceptionMessage}", createDto.Quantity, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InsufficientStockException ex)
            {
                _logger.LogWarning("Erro ao criar abastecimento. Estoque insuficiente para o tipo de combustível: {FuelType}. Exceção: {ExceptionMessage}", createDto.FuelType, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (PriceNotFoundException ex)
            {
                _logger.LogWarning("Erro ao criar abastecimento. Preço não encontrado para o tipo de combustível: {FuelType}. Exceção: {ExceptionMessage}", createDto.FuelType, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado ao criar abastecimento. Exceção: {ExceptionMessage}", ex.Message);
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
            finally
            {
                _logger.LogInformation("Processo de criação de abastecimento finalizado. Tipo de combustível: {FuelType}, Quantidade: {Quantity}, Data: {Date}.",
                    createDto.FuelType, createDto.Quantity, createDto.Date);
            }
        }



        [HttpPatch("{id}")]
        public IActionResult UpdateSupply(int id, [FromBody] UpdateSupplyDTO updateDto)
        {
            _logger.LogInformation("Iniciando atualização de abastecimento. ID: {SupplyId}, Dados Atualizados: {UpdateData}.", id, updateDto);

            try
            {
                var result = _service.UpdateSupply(id, updateDto);

                _logger.LogInformation("Abastecimento atualizado com sucesso. ID: {SupplyId}, Novo Valor Total: {TotalValue}.", result.Id, result.Value);

                return Ok(result);
            }
            catch (SupplyNotFoundException ex)
            {
                _logger.LogWarning("Erro ao atualizar abastecimento. Abastecimento não encontrado. ID: {SupplyId}. Exceção: {ExceptionMessage}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidFuelTypeException ex)
            {
                _logger.LogWarning("Erro ao atualizar abastecimento. Tipo de combustível inválido. Dados fornecidos: {FuelType}. Exceção: {ExceptionMessage}", updateDto.FuelType, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (FuelNotFoundException ex)
            {
                _logger.LogWarning("Erro ao atualizar abastecimento. Combustível não encontrado. Dados fornecidos: {FuelType}. Exceção: {ExceptionMessage}", updateDto.FuelType, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InsufficientStockException ex)
            {
                _logger.LogWarning("Erro ao atualizar abastecimento. Estoque insuficiente para a operação. Tipo de combustível: {FuelType}. Exceção: {ExceptionMessage}", updateDto.FuelType, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidSupplyQuantityException ex)
            {
                _logger.LogWarning("Erro ao atualizar abastecimento. Quantidade inválida. Quantidade fornecida: {Quantity}. Exceção: {ExceptionMessage}", updateDto.Quantity, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado ao atualizar abastecimento. ID: {SupplyId}. Exceção: {ExceptionMessage}", id, ex.Message);
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
            finally
            {
                _logger.LogInformation("Processo de atualização de abastecimento finalizado. ID: {SupplyId}.", id);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteSupply(int id)
        {
            try
            {
                _service.DeleteSupply(id);
                return NoContent();
            }
            catch (SupplyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

        [HttpGet("report/{date}")]
        public IActionResult GetDailyReport(DateTime date)
        {
            try
            {
                var report = _service.GetDailyReport(date);
                return Ok(report);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpGet("type/{fuelType}")]
        public ActionResult<IEnumerable<SupplyDTO>> GetSuppliesByType(string fuelType)
        {
            try
            {
                var supplies = _service.GetSuppliesByType(fuelType);
                return Ok(supplies);
            }
            catch (SupplyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (FuelNotFoundException ex)
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
