using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourManagement.Data;
using TourManagement.Model;
using TourManagement.Services.Interfaces;
using TourManagement.DTOs;
using Microsoft.AspNetCore.OData.Query;

namespace TourManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Yêu cầu có token hợp lệ
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;

        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

        // GET: api/Tours
        [HttpGet]
        [EnableQuery]
        public IQueryable<TourReadDTO> GetToursAsync()
        {
           return _tourService.GetAllToursAsync();
                     
        }

        // GET: api/Tours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tour>> GetTour(Guid id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            return Ok(tour);
        }

        // PUT: api/Tours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTour(Guid id, TourUpdateDTO tour)
        {
            if (id != tour.TourID)
            {
                return BadRequest();
            }
            var success = await _tourService.UpdateTourAsync(tour);
            if (!success) return NotFound();
            return NoContent();
        }

        // POST: api/Tours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tour>> PostTour(TourCreateDTO tour)
        {
            var newTour = await _tourService.CreateTourAsync(tour);

            return CreatedAtAction("GetTour", new { id = newTour.TourID }, newTour);
        }

        // DELETE: api/Tours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(Guid id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
;
            await _tourService.DeleteTourAsync(id);

            return NoContent();
        }

    }
}
