using System.ComponentModel.DataAnnotations;

namespace RoleUpdateManagement_API.DTOs
{
    public class RoleUpdateFormReadDTO
    {
        [Key]
        public Guid FormId { get; set; }
        public int UserId { get; set; }
        public string RoleSelected { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? TaxNumber { get; set; }
        public string? LicenceImg { get; set; }
        public int FormStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
