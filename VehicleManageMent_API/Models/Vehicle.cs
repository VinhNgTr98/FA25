using System.ComponentModel.DataAnnotations;

namespace VehicleManageMent_API.Models
{
    public class Vehicle
    {
        [Key]
        public Guid VehiclesID { get; set; }  // PK

        public Guid VehicleAgencyId { get; set; } // FK -> VehicleAgency

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty; // Name of the vehicle

        [Required, MaxLength(50)]
        public string VehicleType { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string AvailabilityStatus { get; set; } = string.Empty; // Changed to support Unicode

        [MaxLength(500)]
        public string? Description { get; set; } // nvarchar(500), nullable

        [Required, MaxLength(15)]
        public string LicensePlate { get; set; } = string.Empty; // License plate of the vehicle

        [MaxLength(500)]
        public string? ImageUrl { get; set; } // URL for the vehicle image

        [Required]
        public decimal Price { get; set; } // decimal(10,2), not null
    }
}
