using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeManageMent_API.DTOs
{
    public class MotorbikeCreateDto
    {
        [Required]
        public Guid VehicleAgencyId { get; set; }
        public string MotorbikeName { get; set; } = default!;
        public string MotorbikeBrand { get; set; } = default!;
        public string? Transmission { get; set; }
        public string? Fuel { get; set; }
        public int? EngineCC { get; set; }
        public string? LicensePlate { get; set; }
        public string? Status { get; set; } = "Ready";
        public decimal? DailyPrice { get; set; }
        public decimal? HourlyPrice { get; set; }
        public string? Description { get; set; }
    }
}
