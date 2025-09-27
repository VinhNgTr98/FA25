using MapManagement_API.DTOs;
using MapManagement_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapManagement_API.Controllers
{
    //Gọi Google Places Autocomplete API để gợi ý địa điểm khi người dùng nhập input.
    [ApiController]
    [Route("api/[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly GoogleMapsService _maps;
        public PlacesController(GoogleMapsService maps) => _maps = maps;


        [HttpGet("autocomplete")]
        public async Task<IActionResult> Autocomplete([FromQuery] PlaceAutocompleteRequestDto dto, CancellationToken ct)
        => Ok(await _maps.PlaceAutocompleteAsync(dto.Input, dto.Language, dto.Types, dto.LocationBias, ct));
    }
}
