using Microsoft.AspNetCore.Mvc;
using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Services.Interfaces;

namespace TourAgencyManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourAgencyController : ControllerBase
    {
        private readonly ITourAgencyService _service;

        public TourAgencyController(ITourAgencyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var agencies = await _service.GetAllAsync();
            return Ok(agencies);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var agency = await _service.GetByIdAsync(id);
            if (agency == null) return NotFound();
            return Ok(agency);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TourAgencyCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.TourAgencyId }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TourAgencyUpdateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _service.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpGet("by-user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var agency = await _service.GetByUserIdAsync(userId);
            if (agency == null) return NotFound();
            return Ok(agency);
        }

    }
}