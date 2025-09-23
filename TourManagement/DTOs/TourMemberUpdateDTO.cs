using System.ComponentModel.DataAnnotations;

namespace TourManagement.DTOs
{
    public class TourMemberUpdateDTO
    {
        
        public Guid MemberId { get; set; }

        public string TourMemberName { get; set; } = string.Empty;
        public int TourMemberIdNumber { get; set; }
        
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
