using System.ComponentModel.DataAnnotations;

namespace VehicleAgencyManagement_API.DTOs
{
    public class VehicleAgencyCreateDTO
    {
        public int UserId { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
