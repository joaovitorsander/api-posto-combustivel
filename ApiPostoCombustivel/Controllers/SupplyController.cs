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
        public SupplyController(AppDbContext context)
        {
            _service = new SupplyService(context);
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
            try
            {
                var supplyDto = new SupplyDTO
                {
                    FuelType = createDto.FuelType,
                    Quantity = createDto.Quantity,
                    Date = createDto.Date
                };

                var result = _service.AddSupply(supplyDto);
                return CreatedAtAction(nameof(GetSupplyById), new { id = result.Id }, result);
            }
            catch (FuelNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidSupplyQuantityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InsufficientStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PriceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }


        [HttpPatch("{id}")]
        public IActionResult UpdateSupply(int id, [FromBody] UpdateSupplyDTO updateDto)
        {
            try
            {
                var result = _service.UpdateSupply(id, updateDto);
                return Ok(result);
            }
            catch (SupplyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidFuelTypeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FuelNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InsufficientStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidSupplyQuantityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
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
