using Microsoft.AspNetCore.Mvc;
using VehicleManageMent_API.DTOs;
using VehicleManageMent_API.Services.Interfaces;

namespace VehicleManageMent_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _service;

        public VehicleController(IVehicleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleReadDTO>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleReadDTO>> GetById(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleReadDTO>> Create(VehicleCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.VehiclesID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehicleUpdateDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "The dto field is required." });
            }

            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound(new { message = "Vehicle not found." });
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(dto.Name)) entity.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.VehicleType)) entity.VehicleType = dto.VehicleType;
            if (!string.IsNullOrEmpty(dto.AvailabilityStatus)) entity.AvailabilityStatus = dto.AvailabilityStatus;
            if (!string.IsNullOrEmpty(dto.Description)) entity.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.LicensePlate)) entity.LicensePlate = dto.LicensePlate;
            if (!string.IsNullOrEmpty(dto.ImageUrl)) entity.ImageUrl = dto.ImageUrl;
            if (dto.Price > 0) entity.Price = dto.Price;

            var success = await _service.UpdateAsync(id, dto);
            if (!success) return StatusCode(500, new { message = "Failed to update vehicle." });

            return Ok(new { message = "Vehicle updated successfully.", vehicle = entity });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
