using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomManagement_API.DTOs;
using RoomManagement_API.Models;
using RoomManagement_API.Services.Rooms;

namespace RoomManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _svc;
        public RoomsController(IRoomService svc) => _svc = svc;

        // GET api/rooms/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomReadDto>> Get(Guid id, CancellationToken ct)
        {
            var room = await _svc.GetAsync(id, ct);
            return room is null ? NotFound() : Ok(room);
        }

        // GET api/rooms
        [HttpGet]
        public async Task<ActionResult<List<RoomReadDto>>> GetAll(CancellationToken ct)
        {
            var list = await _svc.GetAllAsync(ct);
            return Ok(list);
        }

        // GET api/rooms/by-hotel/{hotelId}
        [HttpGet("by-hotel/{hotelId:guid}")]
        public async Task<ActionResult<List<RoomReadDto>>> GetByHotel(Guid hotelId, CancellationToken ct)
        {
            var list = await _svc.GetByHotelAsync(hotelId, ct);
            return Ok(list);
        }

        // POST api/rooms
        [HttpPost]
        public async Task<ActionResult<RoomReadDto>> Post([FromBody] RoomCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var created = await _svc.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(Get), new { id = created.RoomId }, created);
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(new { error = ex.Message });
            }
        }

        // PUT api/rooms
        [HttpPut]
        public async Task<ActionResult<RoomReadDto>> Put([FromBody] RoomUpdateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                var updated = await _svc.UpdateAsync(dto, ct);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(new { error = ex.Message });
            }
        }

        // DELETE api/rooms/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }

}
