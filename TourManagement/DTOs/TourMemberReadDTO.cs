using System.ComponentModel.DataAnnotations;

namespace TourManagement.DTOs
{
    public class TourMemberReadDTO
    {
        [Key]
        public Guid MemberId { get; set; }

        public string TourMemberName { get; set; } = string.Empty;
        public int TourMemberIdNumber { get; set; }
        public Guid TourId { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
