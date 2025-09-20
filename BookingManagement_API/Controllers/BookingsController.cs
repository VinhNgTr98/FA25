using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookingManagement_API.DTOs;
using BookingManagement_API.Services;

namespace BookingManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _svc;

        public BookingController(IBookingService svc)
        {
            _svc = svc;
        }

        // GET: api/booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingReadDto>>> GetAll(CancellationToken ct)
        {
            var result = await _svc.GetAllAsync(ct);
            return Ok(result);
        }

        // GET: api/booking/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingReadDto>> GetById(int id, CancellationToken ct)
        {
            var booking = await _svc.GetByIdAsync(id, ct);
            return booking == null ? NotFound() : Ok(booking);
        }

        // GET: api/booking/by-user/{userId}
        [HttpGet("by-user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<BookingReadDto>>> GetByUserId(int userId, CancellationToken ct)
        {
            var bookings = await _svc.GetByUserIdAsync(userId, ct);
            return Ok(bookings);
        }

        // GET: api/booking/{bookingId}/items
        [HttpGet("{bookingId:int}/items")]
        public async Task<ActionResult<IEnumerable<BookingItemReadDto>>> GetItemsByBookingId(int bookingId, CancellationToken ct)
        {
            var items = await _svc.GetItemsByOrderIdAsync(bookingId, ct);
            return Ok(items);
        }

        // POST: api/booking
        [HttpPost]
        public async Task<ActionResult<BookingReadDto>> Create([FromBody] BookingCreateDto dto, CancellationToken ct)
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.BookingId }, created);
        }

        // POST: api/booking/{bookingId}/items
        [HttpPost("{bookingId:int}/items")]
        public async Task<ActionResult<BookingItemReadDto>> AddItem(int bookingId, [FromBody] BookingItemCreateDto dto, CancellationToken ct)
        {
            var created = await _svc.AddItemAsync(bookingId, dto, ct);
            if (created == null) return NotFound("Booking not found.");
            return CreatedAtAction(nameof(GetItemsByBookingId), new { bookingId }, created);
        }

        // PUT: api/booking/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookingUpdateDto dto, CancellationToken ct)
        {
            var ok = await _svc.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        // DELETE: api/booking/{id}
        // Chặn xóa nếu trạng thái là Accepted
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var existing = await _svc.GetByIdAsync(id, ct);
            if (existing == null) return NotFound();

            if (string.Equals(existing.Status, "Accepted", StringComparison.OrdinalIgnoreCase))
                return Conflict("Booking has been accepted and cannot be deleted.");

            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}