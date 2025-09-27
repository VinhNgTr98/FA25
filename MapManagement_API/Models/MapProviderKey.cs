namespace MapManagement_API.Models
{
    public class MapProviderKey
    {
        public int Id { get; set; }
        public string Provider { get; set; } = "Google"; // future: support more providers
        public string ApiKey { get; set; } = default!; // store securely in prod
        public bool IsActive { get; set; } = true;
        public string? AllowedReferrers { get; set; } // e.g., domain1.com;domain2.com
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeactivatedAt { get; set; }
    }
}
