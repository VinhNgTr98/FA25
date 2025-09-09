namespace VehicleManageMent_API.DTOs
{
    public class VehicleUpdateDTO
    {
        //public Guid VehicleAgencyId { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public string AvailabilityStatus { get; set; } = string.Empty;
        public int Category { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
