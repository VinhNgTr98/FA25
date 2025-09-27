using System.ComponentModel.DataAnnotations;

namespace MapManagement_API.DTOs
{
    public class GeocodeRequestDto
    {
        [Required]
        public string Address { get; set; } = default!;
        public string? Language { get; set; }
        public string? Region { get; set; }
    }
}
