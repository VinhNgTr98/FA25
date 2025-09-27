namespace MapManagement_API.Models
{
    public class MapRequestLog
    {
        public long Id { get; set; }
        public string Endpoint { get; set; } = default!; 
        public string QueryString { get; set; } = default!;
        public int HttpStatus { get; set; }
        public int LatencyMs { get; set; }
        public string? Caller { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
