using System.ComponentModel.DataAnnotations;

namespace UserManagement_API.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string UsersName { get; set; } = default!;

        public bool IsHotelOwner { get; set; }
        public bool IsTourAgency { get; set; }
        public bool IsVehicleAgency { get; set; }
        public bool IsWebAdmin { get; set; }
        public bool IsSupervisor { get; set; }

        public bool? IsActive { get; set; }  // optional, chỉ admin mới update
    }
}

