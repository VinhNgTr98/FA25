using MapManagement_API.DTOs;
using MapManagement_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapManagement_API.Controllers
{
    // API lấy chỉ đường (directions) từ Google Maps
    [ApiController]
    [Route("api/[controller]")]
    public class DirectionsController : ControllerBase
    {
        private readonly GoogleMapsService _maps;
        public DirectionsController(GoogleMapsService maps) => _maps = maps;


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DirectionsRequestDto dto, CancellationToken ct)
        => Ok(await _maps.DirectionsAsync(dto.Origin, dto.Destination, dto.Mode, dto.Language, ct));
    }
}
