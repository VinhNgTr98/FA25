using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleAgencyManagement_API.Models
{
    public class VehicleAgency
    {
        [Key]
        public Guid VehicleAgencyId { get; set; }

        [Required]
        
        public Guid UserId { get; set; }

        [Required, MaxLength(255)]
        public string Location { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }
    }
}
