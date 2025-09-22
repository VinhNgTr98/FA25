using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeManageMent_API.Models
{

   
    public class Motorbike
    {
        public Guid MotorbikeId { get; set; }
        public Guid VehicleAgencyId { get; set; }

        [Required, MaxLength(100)]
        public string MotorbikeName { get; set; } = default!;


        [Required, MaxLength(50)]
        public string MotorbikeBrand { get; set; } = default!;

        [Required]
        public String? Transmission { get; set; }

        [Required]
        public String? Fuel { get; set; }

        public int? EngineCC { get; set; }

        [MaxLength(20)]
        public string? LicensePlate { get; set; }

        [Required]
        public String? Status { get; set; } = "Ready";

        [Precision(18, 2)]
        public decimal? DailyPrice { get; set; }

        [Precision(18, 2)]
        public decimal? HourlyPrice { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

    }
}
