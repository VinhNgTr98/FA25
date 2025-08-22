using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsersInfoManagement_API.Models
{
    [Table("Users")]
    public class User
    {
        public int UserID { get; set; }
        public string UsersName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsHotelOwner { get; set; }
        public bool IsTourAgency { get; set; }
        public bool IsVehicleAgency { get; set; }
        public bool IsWebAdmin { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsActive { get; set; }
        public int CountWarning { get; set; }
        public DateTime CreatedAt { get; set; }

        public UsersInfo? UsersInfo { get; set; } // 1–1
    }
}
