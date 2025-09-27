using System.ComponentModel.DataAnnotations;

namespace MapManagement_API.DTOs
{
    public class PlaceAutocompleteRequestDto
    {
        [Required]
        public string Input { get; set; } = default!;
        public string? Language { get; set; }
        public string? Types { get; set; } // ví dụ: "address"
        public string? LocationBias { get; set; } // ví dụ: "circle:2000@21.03,105.85"
    }
}
