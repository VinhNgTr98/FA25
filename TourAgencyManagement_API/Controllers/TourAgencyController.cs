using Microsoft.AspNetCore.Mvc;
using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Services;

namespace TourAgencyManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourAgencyController : ControllerBase
    {
        private readonly TourAgencyService _service;

        public TourAgencyController(TourAgencyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourAgencyReadDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TourAgencyReadDTO>> GetById(Guid id)
        {
            var agency = await _service.GetByIdAsync(id);
            if (agency == null) return NotFound();
            return Ok(agency);
        }

        [HttpPost]
        public async Task<ActionResult<TourAgencyReadDTO>> Create(TourAgencyCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.TourAgencyId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TourAgencyUpdateDTO dto)
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
