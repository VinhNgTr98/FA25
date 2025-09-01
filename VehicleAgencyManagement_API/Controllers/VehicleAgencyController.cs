using Microsoft.AspNetCore.Mvc;
using VehicleAgencyManagement_API.DTOs;
using VehicleAgencyManagement_API.Services.Interfaces;

namespace VehicleAgencyManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleAgencyController : ControllerBase
    {
        private readonly IVehicleAgencyService _service;

        public VehicleAgencyController(IVehicleAgencyService service)
        {
            _service = service;
        }

        // GET: api/VehicleAgency
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var agencies = await _service.GetAllAsync();
            return Ok(agencies);
        }

        // GET: api/VehicleAgency/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var agency = await _service.GetByIdAsync(id);
            if (agency == null) return NotFound();
            return Ok(agency);
        }

        // POST: api/VehicleAgency
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleAgencyCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.VehicleAgencyId }, created);
        }

        // PUT: api/VehicleAgency/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehicleAgencyUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        // DELETE: api/VehicleAgency/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
