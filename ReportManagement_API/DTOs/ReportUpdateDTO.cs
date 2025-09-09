namespace ReportManagement_API.DTOs
{
    public class ReportUpdateDTO
    {
        public int? ReportReasonId { get; set; }
        public string? ReportDescription { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
