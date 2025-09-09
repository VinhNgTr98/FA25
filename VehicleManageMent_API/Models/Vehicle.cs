using System.ComponentModel.DataAnnotations;

namespace VehicleManageMent_API.Models
{
    public class Vehicle
    {
        [Key]
        public Guid VehiclesID { get; set; }  // PK

        public Guid VehicleAgencyId { get; set; } // FK -> VehicleAgency

        public string VehicleType { get; set; } = string.Empty; // varchar(50), not null

        public string AvailabilityStatus { get; set; } = string.Empty; // varchar(50), not null

        public int Category { get; set; } // FK -> Categories.CategoryID

        public string? Description { get; set; } // nvarchar(500), nullable

        public decimal Price { get; set; } // decimal(10,2), not null
    }
}
