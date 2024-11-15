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

        public PriceController(AppDbContext context)
        {
            _service = new PriceService(context);
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
                return CreatedAtAction(nameof(GetPriceById), new { id = result.Id }, result);
            }
            catch (DuplicatePriceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidValueException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidPeriodException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FuelNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (StartDateRequiredException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UndefinedEndDateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdatePrice(int id, [FromBody] UpdatePriceDTO updateDto)
        {
            try
            {
                _service.UpdatePrice(id, updateDto);
                return NoContent();
            }
            catch (PriceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidValueException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidPeriodException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatePriceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UndefinedEndDateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno no servidor: " + e.Message);
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
