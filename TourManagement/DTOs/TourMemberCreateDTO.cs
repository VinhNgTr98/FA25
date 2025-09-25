using System.ComponentModel.DataAnnotations;

namespace TourManagement.DTOs
{
    public class TourMemberCreateDTO
    {
        [Required]
        public string TourMemberName { get; set; } = string.Empty;

        [Required]
        public int TourMemberIdNumber { get; set; }

        [Required]
        public Guid TourId { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
