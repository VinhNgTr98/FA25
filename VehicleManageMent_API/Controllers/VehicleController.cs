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
        public async Task<IActionResult> Update(Guid id, VehicleUpdateDTO dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
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
