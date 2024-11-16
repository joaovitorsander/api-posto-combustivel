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
        private readonly PriceService _service; // Serviço que contém a lógica de negócios para os preços.
        private readonly ILogger<PriceController> _logger; // Logger para registrar informações sobre as operações.

        // Construtor que inicializa o serviço de preços e o logger.
        public PriceController(AppDbContext context, ILogger<PriceController> logger)
        {
            _service = new PriceService(context); // Inicializa o serviço com o contexto do banco de dados.
            _logger = logger; // Inicializa o logger para registrar eventos importantes.
        }

        // Endpoint para obter todos os preços cadastrados.
        [HttpGet]
        public ActionResult<IEnumerable<PriceDTO>> GetPrices()
        {
            var prices = _service.GetPrices(); // Busca todos os preços no banco de dados.
            return Ok(prices); // Retorna a lista de preços com status 200 (OK).
        }

        // Endpoint para buscar um preço específico pelo ID.
        [HttpGet("{id:int}")]
        public ActionResult<PriceDTO> GetPriceById(int id)
        {
            try
            {
                var price = _service.GetPriceById(id); // Busca o preço correspondente ao ID fornecido.
                return Ok(price); // Retorna o preço com status 200 (OK).
            }
            catch (PriceNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 (Not Found) se o preço não for encontrado.
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Retorna 500 (Erro interno) para exceções inesperadas.
            }
        }

        // Endpoint para adicionar um novo preço.
        [HttpPost]
        public IActionResult AddPrice([FromBody] CreatePriceDTO createDto)
        {
            _logger.LogInformation("Iniciando o processo de Adição de Preço. Dados recebidos: Combustível ID: {FuelId}, Preço por Litro: {PricePerLiter}, Data Início: {StartDate}, Data Fim: {EndDate}.",
                createDto.FuelId, createDto.PricePerLiter, createDto.StartDate, createDto.EndDate);

            try
            {
                // Cria um DTO para o preço com os dados fornecidos pelo cliente.
                var priceDto = new PriceDTO
                {
                    PricePerLiter = createDto.PricePerLiter,
                    StartDate = createDto.StartDate,
                    EndDate = createDto.EndDate,
                    FuelId = createDto.FuelId
                };

                // Adiciona o preço no banco de dados e retorna o resultado.
                var result = _service.AddPrice(priceDto);

                _logger.LogInformation("Preço adicionado com sucesso. Preço ID: {PriceId}.", result.Id);

                // Retorna 201 (Created) com o preço adicionado.
                return CreatedAtAction(nameof(GetPriceById), new { id = result.Id }, result);
            }
            catch (DuplicatePriceException ex)
            {
                _logger.LogWarning("Tentativa de adicionar preço duplicado para o Combustível ID: {FuelId}. Exceção: {ExceptionMessage}", createDto.FuelId, ex.Message);
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o preço já existir.
            }
            catch (InvalidValueException ex)
            {
                _logger.LogWarning("Valor inválido para o preço. Combustível ID: {FuelId}, Valor: {PricePerLiter}. Exceção: {ExceptionMessage}", createDto.FuelId, createDto.PricePerLiter, ex.Message);
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o valor do preço for inválido.
            }
            catch (InvalidPeriodException ex)
            {
                _logger.LogWarning("Período inválido informado. Data Início: {StartDate}, Data Fim: {EndDate}. Exceção: {ExceptionMessage}", createDto.StartDate, createDto.EndDate, ex.Message);
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o período for inválido.
            }
            catch (FuelNotFoundException ex)
            {
                _logger.LogWarning("Combustível não encontrado. Combustível ID: {FuelId}. Exceção: {ExceptionMessage}", createDto.FuelId, ex.Message);
                return NotFound(ex.Message); // Retorna 404 (Not Found) se o combustível não existir.
            }
            catch (Exception e)
            {
                _logger.LogError("Erro inesperado ao adicionar preço para o Combustível ID: {FuelId}. Exceção: {ExceptionMessage}", createDto.FuelId, e.Message);
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Retorna 500 (Erro interno) para exceções inesperadas.
            }
            finally
            {
                _logger.LogInformation("Processo de Adição de Preço finalizado.");
            }
        }

        // Endpoint para atualizar um preço existente pelo ID.
        [HttpPatch("{id:int}")]
        public IActionResult UpdatePrice(int id, [FromBody] UpdatePriceDTO updateDto)
        {
            _logger.LogInformation("Iniciando atualização do preço. Preço ID: {PriceId}, Dados recebidos: Combustível ID: {FuelId}, Preço por Litro: {PricePerLiter}, Data Início: {StartDate}, Data Fim: {EndDate}.",
                id, updateDto.FuelId, updateDto.PricePerLiter, updateDto.StartDate, updateDto.EndDate);

            try
            {
                // Atualiza o preço no banco de dados com os dados fornecidos.
                _service.UpdatePrice(id, updateDto);

                _logger.LogInformation("Preço atualizado com sucesso. Preço ID: {PriceId}.", id);

                return NoContent(); // Retorna 204 (No Content) se a atualização for bem-sucedida.
            }
            catch (PriceNotFoundException ex)
            {
                _logger.LogWarning("Preço não encontrado. Preço ID: {PriceId}. Exceção: {ExceptionMessage}", id, ex.Message);
                return NotFound(ex.Message); // Retorna 404 (Not Found) se o preço não for encontrado.
            }
            catch (Exception e)
            {
                _logger.LogError("Erro inesperado ao atualizar o preço. Preço ID: {PriceId}. Exceção: {ExceptionMessage}", id, e.Message);
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Retorna 500 (Erro interno) para exceções inesperadas.
            }
            finally
            {
                _logger.LogInformation("Processo de atualização do preço finalizado. Preço ID: {PriceId}.", id);
            }
        }

        // Endpoint para deletar um preço pelo ID.
        [HttpDelete("{id:int}")]
        public IActionResult DeletePrice(int id)
        {
            try
            {
                _service.DeletePrice(id); // Deleta o preço correspondente ao ID fornecido.
                return NoContent(); // Retorna 204 (No Content) se a deleção for bem-sucedida.
            }
            catch (PriceNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 (Not Found) se o preço não for encontrado.
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Retorna 500 (Erro interno) para exceções inesperadas.
            }
        }
    }
}
