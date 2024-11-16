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
    // Define a rota base para o controlador e indica que ele é um controlador de API.
    [Route("api/supplies")]
    [ApiController]
    public class SupplyController : ControllerBase
    {
        // Declara os serviços necessários para o controlador.
        private readonly SupplyService _service;
        private readonly ILogger<SupplyController> _logger;

        // Construtor para injetar dependências.
        public SupplyController(AppDbContext context, ILogger<SupplyController> logger)
        {
            _service = new SupplyService(context);
            _logger = logger;
        }

        // Endpoint para obter todos os abastecimentos.
        [HttpGet]
        public ActionResult<IEnumerable<SupplyDTO>> GetSupplies()
        {
            // Retorna a lista de abastecimentos.
            return Ok(_service.GetSupplies());
        }

        // Endpoint para obter um abastecimento específico pelo ID.
        [HttpGet("{id}")]
        public ActionResult<SupplyDTO> GetSupplyById(int id)
        {
            try
            {
                var supply = _service.GetSupplyById(id);
                return Ok(supply); // Retorna o abastecimento se encontrado.
            }
            catch (SupplyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 se não encontrado.
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message); // Trata erros inesperados.
            }
        }

        // Endpoint para adicionar um novo abastecimento.
        [HttpPost]
        public IActionResult AddSupply([FromBody] CreateSupplyDTO createDto)
        {
            // Log inicial do processo de criação.
            _logger.LogInformation("Iniciando criação de abastecimento. Tipo de combustível: {FuelType}, Quantidade: {Quantity}, Data: {Date}.",
                createDto.FuelType, createDto.Quantity, createDto.Date);

            try
            {
                // Criação do objeto DTO para o abastecimento.
                var supplyDto = new SupplyDTO
                {
                    FuelType = createDto.FuelType,
                    Quantity = createDto.Quantity,
                    Date = createDto.Date
                };

                // Chama o serviço para adicionar o abastecimento.
                var result = _service.AddSupply(supplyDto);

                // Log de sucesso.
                _logger.LogInformation("Abastecimento criado com sucesso. ID: {SupplyId}, Valor Total: {TotalValue}.", result.Id, result.Value);

                // Retorna o recurso criado com o status 201.
                return CreatedAtAction(nameof(GetSupplyById), new { id = result.Id }, result);
            }
            // Tratamento de exceções específicas e log de erros.
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
                // Log de finalização do processo.
                _logger.LogInformation("Processo de criação de abastecimento finalizado. Tipo de combustível: {FuelType}, Quantidade: {Quantity}, Data: {Date}.",
                    createDto.FuelType, createDto.Quantity, createDto.Date);
            }
        }

        // Endpoint para atualizar um abastecimento existente.
        [HttpPatch("{id}")]
        public IActionResult UpdateSupply(int id, [FromBody] UpdateSupplyDTO updateDto)
        {
            // Log inicial do processo de atualização.
            _logger.LogInformation("Iniciando atualização de abastecimento. ID: {SupplyId}, Dados Atualizados: {UpdateData}.", id, updateDto);

            try
            {
                // Chama o serviço para atualizar o abastecimento.
                var result = _service.UpdateSupply(id, updateDto);

                // Log de sucesso.
                _logger.LogInformation("Abastecimento atualizado com sucesso. ID: {SupplyId}, Novo Valor Total: {TotalValue}.", result.Id, result.Value);

                // Retorna o recurso atualizado.
                return Ok(result);
            }
            // Tratamento de exceções específicas e log de erros.
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
                // Log de finalização do processo.
                _logger.LogInformation("Processo de atualização de abastecimento finalizado. ID: {SupplyId}.", id);
            }
        }

        // Endpoint para deletar um abastecimento.
        [HttpDelete("{id}")]
        public IActionResult DeleteSupply(int id)
        {
            try
            {
                // Chama o serviço para deletar o abastecimento.
                _service.DeleteSupply(id);
                return NoContent(); // Retorna 204 em caso de sucesso.
            }
            catch (SupplyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 se não encontrado.
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message); // Trata erros inesperados.
            }
        }

        // Endpoint para gerar relatório diário.
        [HttpGet("report/{date}")]
        public IActionResult GetDailyReport(DateTime date)
        {
            try
            {
                var report = _service.GetDailyReport(date);
                return Ok(report); // Retorna o relatório diário.
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Trata erros inesperados.
            }
        }

        // Endpoint para buscar abastecimentos por tipo de combustível.
        [HttpGet("type/{fuelType}")]
        public ActionResult<IEnumerable<SupplyDTO>> GetSuppliesByType(string fuelType)
        {
            try
            {
                var supplies = _service.GetSuppliesByType(fuelType);
                return Ok(supplies); // Retorna os abastecimentos encontrados.
            }
            catch (SupplyNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 se não encontrado.
            }
            catch (FuelNotFoundException ex)
            {
                return BadRequest(ex.Message); // Retorna 400 para erros de entrada.
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message); // Trata erros inesperados.
            }
        }
    }
}
