using AutoMapper;
using HotelsManagement_API.Data;
using HotelsManagement_API.DTOs;
using HotelsManagement_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly HotelsManagement_APIContext _db;
    private readonly IMapper _mapper;

    public HotelsController(HotelsManagement_APIContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelReadDto>>> GetAll()
    {
        var hotels = await _db.Hotel.AsNoTracking().ToListAsync();
        return Ok(_mapper.Map<IEnumerable<HotelReadDto>>(hotels));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HotelReadDto>> GetById(Guid id)
    {
        var hotel = await _db.Hotel.AsNoTracking().FirstOrDefaultAsync(h => h.HotelId == id);
        if (hotel == null) return NotFound();
        return Ok(_mapper.Map<HotelReadDto>(hotel));
    }
    [HttpGet("GetByUserID/{id:int}")]
    public async Task<ActionResult<HotelReadDto>> GetByUserID(int id)
    {
        var hotel = await _db.Hotel.AsNoTracking().FirstOrDefaultAsync(h => h.UserID == id);
        if (hotel == null) return NotFound();
        return Ok(_mapper.Map<HotelReadDto>(hotel));
    }

    [HttpPost]
    public async Task<ActionResult<HotelReadDto>> Create([FromBody] HotelCreateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var entity = _mapper.Map<Hotel>(dto);
        entity.HotelId = Guid.NewGuid();

        await _db.Hotel.AddAsync(entity);
        await _db.SaveChangesAsync();

        var result = _mapper.Map<HotelReadDto>(entity);
        return CreatedAtAction(nameof(GetById), new { id = result.HotelId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] HotelUpdateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (id != dto.HotelId) return BadRequest("Route id != body HotelId");

        var current = await _db.Hotel.FirstOrDefaultAsync(h => h.HotelId == id);
        if (current == null) return NotFound();

        _mapper.Map(dto, current);
        _db.Hotel.Update(current);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var current = await _db.Hotel.FirstOrDefaultAsync(h => h.HotelId == id);
        if (current == null) return NotFound();

        _db.Hotel.Remove(current);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
