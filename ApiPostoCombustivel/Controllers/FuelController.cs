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
        private readonly FuelService _service; // Serviço que contém a lógica de negócios para combustíveis.
        private readonly ILogger<FuelController> _logger; // Logger para registrar informações sobre as operações.

        // Construtor que inicializa o serviço de combustíveis e o logger.
        public FuelController(AppDbContext context, ILogger<FuelController> logger)
        {
            _service = new FuelService(context); // Inicializa o serviço com o contexto do banco de dados.
            _logger = logger; // Inicializa o logger para registrar eventos importantes.
        }

        // Endpoint para obter o estoque de todos os combustíveis.
        [HttpGet]
        public ActionResult<IEnumerable<FuelDTO>> GetInventory()
        {
            var stock = _service.GetInventory(); // Busca o estoque completo de combustíveis.
            return Ok(stock); // Retorna o estoque com status 200 (OK).
        }

        // Endpoint para buscar um combustível pelo ID.
        [HttpGet("{id:int}")]
        public ActionResult<FuelDTO> GetFuelById(int id)
        {
            try
            {
                var fuel = _service.GetFuelById(id); // Busca o combustível pelo ID.
                return Ok(fuel); // Retorna o combustível com status 200 (OK).
            }
            catch (FuelNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 (Not Found) se o combustível não for encontrado.
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Retorna 500 (Erro interno) para exceções inesperadas.
            }
        }

        // Endpoint para buscar um combustível pelo tipo (ex: Gasolina, Diesel).
        [HttpGet("tipo/{type}")]
        public ActionResult<FuelDTO> GetFuelByType(string type)
        {
            try
            {
                var fuel = _service.GetFuelByType(type); // Busca o combustível pelo tipo fornecido.
                return Ok(fuel); // Retorna o combustível com status 200 (OK).
            }
            catch (InvalidFuelTypeException ex)
            {
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o tipo fornecido for inválido.
            }
            catch (FuelNotFoundException ex)
            {
                return NotFound(ex.Message); // Retorna 404 (Not Found) se o combustível não for encontrado.
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Retorna 500 (Erro interno) para exceções inesperadas.
            }
        }

        // Endpoint para adicionar um novo tipo de combustível.
        [HttpPost]
        public IActionResult AddFuel([FromBody] CreateFuelDTO createDto)
        {
            _logger.LogInformation("Iniciando o processo de Adicionar Combustível."); // Log de início do processo.

            try
            {
                _logger.LogInformation("Tentando adicionar um novo tipo de combustível: {FuelType} com estoque: {Stock}.", createDto.Type, createDto.Stock);

                // Cria um DTO para o combustível com os dados fornecidos.
                var fuelDto = new FuelDTO
                {
                    Type = createDto.Type,
                    Stock = createDto.Stock
                };

                // Adiciona o combustível e obtém o resultado.
                var result = _service.AddFuel(fuelDto);

                _logger.LogInformation("Tipo de combustível adicionado com sucesso: {FuelType} com ID: {FuelId}.", result.Type, result.Id);

                return CreatedAtAction(nameof(GetFuelById), new { id = result.Id }, result); // Retorna 201 (Created) com o combustível adicionado.
            }
            catch (FuelAlreadyRegisteredException ex)
            {
                _logger.LogWarning("Tipo de combustível já registrado: {FuelType}. Exceção: {ExceptionMessage}", createDto.Type, ex.Message);
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o combustível já estiver registrado.
            }
            catch (InvalidFuelTypeException ex)
            {
                _logger.LogWarning("Tipo de combustível inválido: {FuelType}. Exceção: {ExceptionMessage}", createDto.Type, ex.Message);
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o tipo de combustível for inválido.
            }
            catch (InvalidStockException ex)
            {
                _logger.LogWarning("Valor de estoque inválido para o tipo de combustível: {FuelType}. Exceção: {ExceptionMessage}", createDto.Type, ex.Message);
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o estoque for inválido.
            }
            catch (Exception e)
            {
                _logger.LogError("Erro inesperado ao adicionar o tipo de combustível: {FuelType}. Exceção: {ExceptionMessage}", createDto.Type, e.Message);
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Retorna 500 (Erro interno) para exceções inesperadas.
            }
            finally
            {
                _logger.LogInformation("Processo de Adicionar Combustível finalizado."); // Log de finalização do processo.
            }
        }

        // Endpoint para atualizar os dados de um combustível existente.
        [HttpPatch("{id:int}")]
        public IActionResult UpdateFuel(int id, [FromBody] UpdateFuelDTO updateDto)
        {
            _logger.LogInformation("Iniciando o processo de Atualização do Combustível com ID: {FuelId}.", id); // Log de início do processo.

            try
            {
                _logger.LogInformation("Atualizando o combustível com ID: {FuelId}. Dados recebidos: Tipo: {FuelType}, Estoque: {Stock}.",
                    id, updateDto.Type, updateDto.Stock);

                // Atualiza o combustível com os dados fornecidos.
                _service.UpdateFuel(id, updateDto);

                _logger.LogInformation("Combustível com ID: {FuelId} atualizado com sucesso.", id); // Log de sucesso.

                return NoContent(); // Retorna 204 (No Content) se a atualização for bem-sucedida.
            }
            catch (FuelNotFoundException ex)
            {
                _logger.LogWarning("Combustível não encontrado para o ID: {FuelId}. Exceção: {ExceptionMessage}", id, ex.Message);
                return NotFound(ex.Message); // Retorna 404 (Not Found) se o combustível não for encontrado.
            }
            catch (InvalidFuelTypeException ex)
            {
                _logger.LogWarning("Tipo de combustível inválido para o ID: {FuelId}. Tipo informado: {FuelType}. Exceção: {ExceptionMessage}",
                    id, updateDto.Type, ex.Message);
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o tipo de combustível for inválido.
            }
            catch (InvalidStockException ex)
            {
                _logger.LogWarning("Valor de estoque inválido para o ID: {FuelId}. Estoque informado: {Stock}. Exceção: {ExceptionMessage}",
                    id, updateDto.Stock, ex.Message);
                return BadRequest(ex.Message); // Retorna 400 (Bad Request) se o estoque for inválido.
            }
            catch (Exception e)
            {
                _logger.LogError("Erro inesperado ao atualizar o combustível com ID: {FuelId}. Exceção: {ExceptionMessage}", id, e.Message);
                return StatusCode(500, "Erro interno no servidor: " + e.Message); // Retorna 500 (Erro interno) para exceções inesperadas.
            }
            finally
            {
                _logger.LogInformation("Processo de Atualização do Combustível finalizado para o ID: {FuelId}.", id); // Log de finalização do processo.
            }
        }

        // Endpoint para deletar um combustível pelo ID.
        [HttpDelete("{id:int}")]
        public IActionResult DeleteFuel(int id)
        {
            // Inicia o processo de deleção do combustível pelo ID informado.
            try
            {
                // Chama o serviço para deletar o combustível correspondente ao ID.
                _service.DeleteFuel(id);

                // Se a deleção for bem-sucedida, retorna um status 204 (No Content).
                return NoContent();
            }
            catch (FuelNotFoundException ex)
            {
                // Caso o combustível não seja encontrado, retorna um status 404 (Not Found) com a mensagem da exceção.
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                // Captura qualquer outra exceção inesperada, retorna um status 500 (Erro interno no servidor)
                // e inclui a mensagem de erro na resposta.
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }
    }
}