using Microsoft.AspNetCore.Mvc;

namespace MapManagement_API.Controllers
{

    // API tạo URL nhúng (embed) bản đồ từ Google Maps
    [ApiController]
    [Route("api/[controller]")]
    public class EmbedController : ControllerBase
    {
        private readonly IConfiguration _config;
        public EmbedController(IConfiguration config) => _config = config;
        // key cho browser 
        [HttpGet("place")]
        public IActionResult GetPlaceEmbedUrl([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Query is required");

            var key = _config["GoogleMaps:ApiKey"]; // key cho browser/embed
            var url = $"https://www.google.com/maps/embed/v1/place?key={key}&q={Uri.EscapeDataString(q)}";
            return Ok(new { embedUrl = url });
        }
        // điểm đi và điểm đến
        [HttpGet("directions")]
        public IActionResult GetDirectionsEmbedUrl([FromQuery] string origin, [FromQuery] string destination)
        {
            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
                return BadRequest("Origin and destination are required");

            var key = _config["GoogleMaps:ApiKey"];
            var url = $"https://www.google.com/maps/embed/v1/directions?key={key}&origin={Uri.EscapeDataString(origin)}&destination={Uri.EscapeDataString(destination)}";
            return Ok(new { embedUrl = url });
        }
    }

}
