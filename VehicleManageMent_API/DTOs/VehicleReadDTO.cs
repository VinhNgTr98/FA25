using System.ComponentModel.DataAnnotations;

namespace VehicleManageMent_API.DTOs
{
    public class VehicleReadDTO
    {
        [Key]
        public Guid VehiclesID { get; set; }
        public Guid VehicleAgencyId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty; // Name of the vehicle

        public string VehicleType { get; set; } = string.Empty;
        public string AvailabilityStatus { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required, MaxLength(15)]
        public string LicensePlate { get; set; } = string.Empty; // License plate of the vehicle

        [MaxLength(500)]
        public string? ImageUrl { get; set; } // URL for the vehicle image

        public decimal Price { get; set; }
    }
}
