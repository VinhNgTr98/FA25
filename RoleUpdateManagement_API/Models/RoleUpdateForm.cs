using System.ComponentModel.DataAnnotations;

namespace RoleUpdateManagement_API.Models
{
    public class RoleUpdateForm
    {
        [Key]
        public Guid FormId { get; set; }

        // Foreign Key (chỉ lưu ID, không navigation)
        public int UserId { get; set; }

        public string RoleSelected { get; set; } = string.Empty; // HotelOwner, TourAgency, VehicleAgency

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? TaxNumber { get; set; }

        public string? LicenceImg { get; set; }

        public int FormStatus { get; set; } // 0 = Pending, 1 = Approved, 2 = Rejected

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
