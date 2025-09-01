namespace TourAgencyManagement_API.DTOs
{
    public class TourAgencyCreateDTO
    {
        public Guid UserId { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
