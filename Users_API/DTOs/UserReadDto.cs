using System.ComponentModel.DataAnnotations;

namespace User_API.DTOs
{
    public class UserReadDto
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [StringLength(100, ErrorMessage = "UserName must be less than 100 characters")]
        public string UserName { get; set; }

        public bool IsHotelOwner { get; set; }
        public bool IsTourAgency { get; set; }
        public bool IsVehicleAgency { get; set; }
        public bool IsWebAdmin { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsActive { get; set; }
        public int CountWarning { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
