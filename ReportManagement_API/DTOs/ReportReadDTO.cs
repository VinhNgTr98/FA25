using System.ComponentModel.DataAnnotations;

namespace ReportManagement_API.DTOs
{
    public class ReportReadDTO
    {
        [Key]
        public int ReportId { get; set; }
        public int UserID { get; set; }
        public Guid ReportedID { get; set; }
        public string ReportType { get; set; } = string.Empty;
        public int? ReportReasonId { get; set; }
        public string ReportDescription { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
