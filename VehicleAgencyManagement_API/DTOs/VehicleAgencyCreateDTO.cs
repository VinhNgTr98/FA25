namespace VehicleAgencyManagement_API.DTOs
{
    public class VehicleAgencyCreateDTO
    {
        public Guid UserId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
