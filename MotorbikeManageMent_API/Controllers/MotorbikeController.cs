using Microsoft.AspNetCore.Mvc;
using MotorbikeManageMent_API.DTOs;
using MotorbikeManageMent_API.Services.Interfaces;

namespace MotorbikeManageMent_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorbikeController : ControllerBase
    {
        private readonly IMotorbikeService _service;

        public MotorbikeController(IMotorbikeService service)
        {
            _service = service;
        }

        // Các phương thức CRUD cơ bản
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotorbikeReadDto>>> GetAllMotorbikes()
        {
            var motorbikes = await _service.GetAllAsync();
            return Ok(motorbikes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MotorbikeReadDto>> GetMotorbike(Guid id)
        {
            var motorbike = await _service.GetByIdAsync(id);
            if (motorbike == null)
            {
                return NotFound();
            }
            return Ok(motorbike);
        }

        // Phương thức lọc theo Transmission, Fuel, MotorbikeBrand, MotorbikeName, EngineCc
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<MotorbikeReadDto>>> GetFilteredMotorbikes(
            [FromQuery] string? transmission,
            [FromQuery] string? fuel,
            [FromQuery] string? motorbikeBrand,
            [FromQuery] string? motorbikeName,
            [FromQuery] int? engineCc)
        {
            var motorbikes = await _service.GetFilteredMotorbikesAsync(transmission, fuel, motorbikeBrand, motorbikeName, engineCc);
            return Ok(motorbikes);
        }

        [HttpPost]
        public async Task<ActionResult<MotorbikeReadDto>> CreateMotorbike([FromBody] MotorbikeCreateDto dto)
        {
            var motorbike = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetMotorbike), new { id = motorbike.MotorbikeId }, motorbike);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMotorbike(Guid id, [FromBody] MotorbikeUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotorbike(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
