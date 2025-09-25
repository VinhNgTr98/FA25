using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class UpdateUserRoleDto
    {
        [Required]
        public int UserID { get; set; }

        public bool? IsHotelOwner { get; set; } = null;
        public bool? IsTourAgency { get; set; } = null;
        public bool? IsVehicleAgency { get; set; } = null;

        // Lý do bắt buộc khi Value = false (tắt/từ chối)
        [MaxLength(400)]
        public string? RejectedNote { get; set; }
    }

}
