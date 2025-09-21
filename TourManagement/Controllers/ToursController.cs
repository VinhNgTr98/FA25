using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using TourManagement.DTOs;
using TourManagement.Services.Interfaces;

namespace TourManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly IItineraryService _itineraryService;

        public TourController(ITourService tourService, IItineraryService itineraryService)
        {
            _tourService = tourService;
            _itineraryService = itineraryService;
        }

        // ---------------------- TOUR ----------------------

        [HttpGet]
        public IActionResult GetAllTours()
        {
            var tours = _tourService.GetAllToursAsync();
            return Ok(tours);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTourById(Guid id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null) return NotFound();
            return Ok(tour);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] TourCreateDTO dto)
        {
            var created = await _tourService.CreateTourAsync(dto);
            return CreatedAtAction(nameof(GetTourById), new { id = created.TourID }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTour(Guid id, [FromBody] TourUpdateDTO dto)
        {
            if (id != dto.TourID) return BadRequest("ID mismatch");

            var result = await _tourService.UpdateTourAsync(dto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTour(Guid id)
        {
            var result = await _tourService.DeleteTourAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        // ---------------------- ITINERARY ----------------------

        [HttpGet("{tourId:guid}/itineraries")]
        public async Task<IActionResult> GetItinerariesByTour(Guid tourId)
        {
            var itineraries = await _itineraryService.GetByTourIdAsync(tourId);
            return Ok(itineraries);
        }

        [HttpGet("itinerary/{id:guid}")]
        public async Task<IActionResult> GetItineraryById(Guid id)
        {
            var itinerary = await _itineraryService.GetByIdAsync(id);
            if (itinerary == null) return NotFound();
            return Ok(itinerary);
        }

        [HttpPost("{tourId:guid}/itineraries")]
        public async Task<IActionResult> CreateItinerary(Guid tourId, [FromBody] ItineraryCreateDTO dto)
        {
            if (tourId != dto.TourID) return BadRequest("TourID mismatch");

            var created = await _itineraryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetItineraryById), new { id = created.ItineraryId }, created);
        }

        [HttpPut("itinerary/{id:guid}")]
        public async Task<IActionResult> UpdateItinerary(Guid id, [FromBody] ItineraryUpdateDTO dto)
        {
            if (id != dto.ItineraryId) return BadRequest("ID mismatch");

            var result = await _itineraryService.UpdateAsync(id, dto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("itinerary/{id:guid}")]
        public async Task<IActionResult> DeleteItinerary(Guid id)
        {
            var result = await _itineraryService.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}