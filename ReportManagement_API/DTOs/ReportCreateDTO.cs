namespace ReportManagement_API.DTOs
{
    public class ReportCreateDTO
    {
        public int UserID { get; set; }
        public Guid ReportedID { get; set; }
        public string ReportType { get; set; } = string.Empty;
        public int? ReportReasonId { get; set; }
        public string ReportDescription { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
