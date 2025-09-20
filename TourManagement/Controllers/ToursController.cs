using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using TourManagement.DTOs;
using TourManagement.Services.Interfaces;

namespace TourManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;

        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

        // GET: api/Tours
        // Hỗ trợ OData filter/sort/select/top qua [EnableQuery] trên IQueryable<TourReadDTO>
        [HttpGet]
        [EnableQuery(PageSize = 100)] // giới hạn mặc định
        public IQueryable<TourReadDTO> GetTours()
        {
            return _tourService.GetAllToursAsync();
        }

        // GET: api/Tours/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TourReadDTO>> GetTour(Guid id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null) return NotFound();
            return Ok(tour);
        }

        // PUT: api/Tours/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutTour(Guid id, [FromBody] TourUpdateDTO tour)
        {
            if (id != tour.TourID) return BadRequest("Route id khác body id.");

            var success = await _tourService.UpdateTourAsync(tour);
            if (!success) return NotFound();
            return NoContent();
        }

        // POST: api/Tours
        [HttpPost]
        public async Task<ActionResult<TourReadDTO>> PostTour([FromBody] TourCreateDTO tour)
        {
            var newTour = await _tourService.CreateTourAsync(tour);
            return CreatedAtAction(nameof(GetTour), new { id = newTour.TourID }, newTour);
        }

        // DELETE: api/Tours/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTour(Guid id)
        {
            var existed = await _tourService.GetTourByIdAsync(id);
            if (existed == null) return NotFound();

            var ok = await _tourService.DeleteTourAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}