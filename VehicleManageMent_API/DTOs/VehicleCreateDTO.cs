namespace VehicleManageMent_API.DTOs
{
    public class VehicleCreateDTO
    {
        public Guid VehicleAgencyId { get; set; }

        public string Name { get; set; } = string.Empty; // Name of the vehicle

        public string VehicleType { get; set; } = string.Empty;

        public string AvailabilityStatus { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string LicensePlate { get; set; } = string.Empty; // License plate of the vehicle

        public string? ImageUrl { get; set; } // URL for the vehicle image

        public decimal Price { get; set; }
    }
}
