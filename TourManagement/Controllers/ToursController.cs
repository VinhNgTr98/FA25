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
        private readonly ITourGuideService _tourGuideService;
        private readonly ITourMemberService _tourMemberService;
        public TourController(ITourService tourService, IItineraryService itineraryService, ITourGuideService tourGuideService, ITourMemberService tourMemberService)
        {
            _tourService = tourService;
            _itineraryService = itineraryService;
            _tourGuideService = tourGuideService;
            _tourMemberService = tourMemberService;
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
        // ---------------------- TOUR GUIDE ----------------------

        [HttpGet("{tourId:guid}/guide")]
        public async Task<IActionResult> GetGuideByTour(Guid tourId)
        {
            var tour = await _tourService.GetTourByIdAsync(tourId);
            if (tour == null) return NotFound("Tour not found");

            var guide = await _tourGuideService.GetByIdAsync(tour.TourGuideId);
            if (guide == null) return NotFound("Tour guide not found");

            return Ok(guide);
        }

        [HttpGet("guides/{id:guid}")]
        public async Task<IActionResult> GetGuideById(Guid id)
        {
            var guide = await _tourGuideService.GetByIdAsync(id);
            if (guide == null) return NotFound();
            return Ok(guide);
        }
        [HttpPost("{tourId:guid}/guide")]
        public async Task<IActionResult> AssignGuideToTour(Guid tourId, [FromBody] TourGuideCreateDTO dto)
        {
            // Không cần check dto.TourId nữa vì model không có TourId
            var created = await _tourGuideService.CreateAsync(dto);

            // Gắn guide vào tour
            var updated = await _tourService.AssignGuideToTourAsync(tourId, created.TourGuideId);
            if (!updated) return BadRequest("Failed to assign guide to tour");

            return CreatedAtAction(nameof(GetGuideById), new { id = created.TourGuideId }, created);
        }


        [HttpPut("guides/{id:guid}")]
        public async Task<IActionResult> UpdateGuide(Guid id, [FromBody] TourGuideUpdateDTO dto)
        {
            if (id != dto.TourGuideId) return BadRequest("ID mismatch");

            var result = await _tourGuideService.UpdateAsync(dto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("guides/{id:guid}")]
        public async Task<IActionResult> DeleteGuide(Guid id)
        {
            var result = await _tourGuideService.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        // ---------------------- TOUR MEMBER ----------------------

        [HttpGet("{tourId:guid}/members")]
        public async Task<IActionResult> GetMembersByTour(Guid tourId)
        {
            var members = await _tourMemberService.GetAllAsync();
            var tourMembers = members.Where(m => m.TourId == tourId);
            return Ok(tourMembers);
        }

        [HttpGet("members/{id:guid}")]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            var member = await _tourMemberService.GetByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPost("{tourId:guid}/members")]
        public async Task<IActionResult> CreateMember(Guid tourId, [FromBody] TourMemberCreateDTO dto)
        {
            if (tourId != dto.TourId) return BadRequest("TourID mismatch");

            var created = await _tourMemberService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetMemberById), new { id = created.MemberId }, created);
        }

        [HttpPut("members/{id:guid}")]
        public async Task<IActionResult> UpdateMember(Guid id, [FromBody] TourMemberUpdateDTO dto)
        {
            if (id != dto.MemberId) return BadRequest("ID mismatch");

            var result = await _tourMemberService.UpdateAsync(dto);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("members/{id:guid}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var result = await _tourMemberService.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

    }
}