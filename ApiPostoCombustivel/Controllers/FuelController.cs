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

        public FuelController(AppDbContext context)
        {
            _service = new FuelService(context);
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
            try
            {
                var fuelDto = new FuelDTO
                {
                    Type = createDto.Type,
                    Stock = createDto.Stock
                };

                var result = _service.AddFuel(fuelDto);
                return CreatedAtAction(nameof(GetFuelById), new { id = result.Id }, result);
            }
            catch (FuelAlreadyRegisteredException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidFuelTypeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateFuel(int id, [FromBody] UpdateFuelDTO updateDto)
        {
            try
            {
                _service.UpdateFuel(id, updateDto);
                return NoContent();
            }
            catch (FuelNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidFuelTypeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
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
