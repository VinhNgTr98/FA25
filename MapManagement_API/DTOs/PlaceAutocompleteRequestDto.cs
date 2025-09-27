using System.ComponentModel.DataAnnotations;

namespace MapManagement_API.DTOs
{
    public class PlaceAutocompleteRequestDto
    {
        [Required]
        public string Input { get; set; } = default!;
        public string? Language { get; set; }
        public string? Types { get; set; } 
        public string? LocationBias { get; set; } 
    }
}
