using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.DTO.PrecoDTO;
using ApiPostoCombustivel.Exceptions;
using ApiPostoCombustivel.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPostoCombustivel.Controllers
{
    [Route("api/prices")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly PriceService _service;
        private readonly ILogger<PriceController> _logger;

        public PriceController(AppDbContext context, ILogger<PriceController> logger)
        {
            _service = new PriceService(context);
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PriceDTO>> GetPrices()
        {
            var prices = _service.GetPrices();
            return Ok(prices);
        }

        [HttpGet("{id:int}")]
        public ActionResult<PriceDTO> GetPriceById(int id)
        {
            try
            {
                var price = _service.GetPriceById(id);
                return Ok(price);
            }
            catch (PriceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpPost]
        public IActionResult AddPrice([FromBody] CreatePriceDTO createDto)
        {
            _logger.LogInformation("Iniciando o processo de Adição de Preço. Dados recebidos: Combustível ID: {FuelId}, Preço por Litro: {PricePerLiter}, Data Início: {StartDate}, Data Fim: {EndDate}.",
                createDto.FuelId, createDto.PricePerLiter, createDto.StartDate, createDto.EndDate);

            try
            {
                var priceDto = new PriceDTO
                {
                    PricePerLiter = createDto.PricePerLiter,
                    StartDate = createDto.StartDate,
                    EndDate = createDto.EndDate,
                    FuelId = createDto.FuelId
                };

                var result = _service.AddPrice(priceDto);

                _logger.LogInformation("Preço adicionado com sucesso. Preço ID: {PriceId}.", result.Id);

                return CreatedAtAction(nameof(GetPriceById), new { id = result.Id }, result);
            }
            catch (DuplicatePriceException ex)
            {
                _logger.LogWarning("Tentativa de adicionar preço duplicado para o Combustível ID: {FuelId}. Exceção: {ExceptionMessage}", createDto.FuelId, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidValueException ex)
            {
                _logger.LogWarning("Valor inválido para o preço. Combustível ID: {FuelId}, Valor: {PricePerLiter}. Exceção: {ExceptionMessage}", createDto.FuelId, createDto.PricePerLiter, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidPeriodException ex)
            {
                _logger.LogWarning("Período inválido informado. Data Início: {StartDate}, Data Fim: {EndDate}. Exceção: {ExceptionMessage}", createDto.StartDate, createDto.EndDate, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (FuelNotFoundException ex)
            {
                _logger.LogWarning("Combustível não encontrado. Combustível ID: {FuelId}. Exceção: {ExceptionMessage}", createDto.FuelId, ex.Message);
                return NotFound(ex.Message);
            }
            catch (StartDateRequiredException ex)
            {
                _logger.LogWarning("Data de início não fornecida. Exceção: {ExceptionMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (UndefinedEndDateException ex)
            {
                _logger.LogWarning("Tentativa de adicionar preço com Data Fim indefinida para o Combustível ID: {FuelId}. Exceção: {ExceptionMessage}", createDto.FuelId, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Erro inesperado ao adicionar preço para o Combustível ID: {FuelId}. Exceção: {ExceptionMessage}", createDto.FuelId, e.Message);
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
            finally
            {
                _logger.LogInformation("Processo de Adição de Preço finalizado.");
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdatePrice(int id, [FromBody] UpdatePriceDTO updateDto)
        {
            _logger.LogInformation("Iniciando atualização do preço. Preço ID: {PriceId}, Dados recebidos: Combustível ID: {FuelId}, Preço por Litro: {PricePerLiter}, Data Início: {StartDate}, Data Fim: {EndDate}.",
                id, updateDto.FuelId, updateDto.PricePerLiter, updateDto.StartDate, updateDto.EndDate);

            try
            {
                _service.UpdatePrice(id, updateDto);

                _logger.LogInformation("Preço atualizado com sucesso. Preço ID: {PriceId}.", id);

                return NoContent();
            }
            catch (PriceNotFoundException ex)
            {
                _logger.LogWarning("Preço não encontrado. Preço ID: {PriceId}. Exceção: {ExceptionMessage}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidValueException ex)
            {
                _logger.LogWarning("Valor inválido informado para o preço. Preço ID: {PriceId}, Valor: {PricePerLiter}. Exceção: {ExceptionMessage}", id, updateDto.PricePerLiter, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidPeriodException ex)
            {
                _logger.LogWarning("Período inválido informado para o preço. Preço ID: {PriceId}, Data Início: {StartDate}, Data Fim: {EndDate}. Exceção: {ExceptionMessage}", id, updateDto.StartDate, updateDto.EndDate, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (DuplicatePriceException ex)
            {
                _logger.LogWarning("Tentativa de duplicar preço. Preço ID: {PriceId}. Exceção: {ExceptionMessage}", id, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (UndefinedEndDateException ex)
            {
                _logger.LogWarning("Tentativa de definir Data Fim indefinida para o Preço ID: {PriceId}. Exceção: {ExceptionMessage}", id, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Erro inesperado ao atualizar o preço. Preço ID: {PriceId}. Exceção: {ExceptionMessage}", id, e.Message);
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
            finally
            {
                _logger.LogInformation("Processo de atualização do preço finalizado. Preço ID: {PriceId}.", id);
            }
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeletePrice(int id)
        {
            try
            {
                _service.DeletePrice(id);
                return NoContent();
            }
            catch (PriceNotFoundException ex)
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
