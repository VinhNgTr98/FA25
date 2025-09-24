using System.ComponentModel.DataAnnotations;

namespace VehicleAgencyManagement_API.DTOs
{
    public class VehicleAgencyReadDTO
    {
        [Key]
        public Guid VehicleAgencyId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty; // Name of the agency

        public int UserId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
