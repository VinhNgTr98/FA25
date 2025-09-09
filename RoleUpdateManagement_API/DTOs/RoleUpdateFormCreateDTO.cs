namespace RoleUpdateManagement_API.DTOs
{
    public class RoleUpdateFormCreateDTO
    {
        public int UserId { get; set; }
        public string RoleSelected { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? TaxNumber { get; set; }
        public string? LicenceImg { get; set; }
    }
}
