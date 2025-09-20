namespace ImageManagement_API.DTOs
{
    public class ImageUploadResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? PublicId { get; set; }
    }
}
