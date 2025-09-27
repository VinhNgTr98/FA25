using MapManagement_API.DTOs;
using MapManagement_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapManagement_API.Controllers
{

    // Gọi Google Geocoding API để chuyển đổi từ địa chỉ sang tọa độ lat/lng (hoặc ngược lại nếu mở rộng)
    [ApiController]
    [Route("api/[controller]")]
    public class GeocodingController : ControllerBase
    {
        private readonly GoogleMapsService _maps;
        public GeocodingController(GoogleMapsService maps) => _maps = maps;


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GeocodeRequestDto dto, CancellationToken ct)
        => Ok(await _maps.GeocodeAsync(dto.Address, dto.Language, dto.Region, ct));
    }
}
