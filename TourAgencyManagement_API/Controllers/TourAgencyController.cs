using Microsoft.AspNetCore.Mvc;
using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Services;
using TourAgencyManagement_API.Services.Interfaces;

namespace TourAgencyManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourAgencyController : ControllerBase
    {
        private readonly ITourAgencyService _service;

        public TourAgencyController(ITourAgencyService service) // ✅ dùng interface
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var agencies = await _service.GetAllAsync();
            return Ok(agencies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var agency = await _service.GetByIdAsync(id);
            if (agency == null) return NotFound();
            return Ok(agency);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TourAgencyCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TourAgencyUpdateDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }

}
