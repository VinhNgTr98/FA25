using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportManagement_API.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]        
        public int UserID { get; set; }

        [Required]
        public Guid ReportedID { get; set; }

        [Required]
        [MaxLength(50)]
        public string ReportType { get; set; } = string.Empty;

        public int? ReportReasonId { get; set; }

        [Required]
        [MaxLength(500)]
        public string ReportDescription { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

    }
}
