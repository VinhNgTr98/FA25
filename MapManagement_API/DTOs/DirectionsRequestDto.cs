using System.ComponentModel.DataAnnotations;

namespace MapManagement_API.DTOs
{
    public class DirectionsRequestDto
    {
        [Required]
        public string Origin { get; set; } = default!; // "21.03,105.85" hoặc địa chỉ


        [Required]
        public string Destination { get; set; } = default!;


        public string? Mode { get; set; } = "driving"; // driving|walking|bicycling|transit
        public string? Language { get; set; }
    }
}
