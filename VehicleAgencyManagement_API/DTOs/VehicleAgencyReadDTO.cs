using System.ComponentModel.DataAnnotations;

namespace VehicleAgencyManagement_API.DTOs
{
    public class VehicleAgencyReadDTO
    {
        [Key]
        public Guid VehicleAgencyId { get; set; }
        public Guid UserId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
