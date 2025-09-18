using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FeedbackManagement_API.DTOs
{
    public class FeedbackReadDTO
    {
        public int FeedbackId { get; init; }
        public int UserID { get; init; } 
        public Guid LinkedId { get; init; }
        public string LinkedType { get; init; } = default!;
        public bool IsReply { get; init; }
        public int? ReplyId { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Rating { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Comment { get; init; }

        public DateTime CreatedAt { get; init; }

        public int RepliesCount { get; init; }

        public List<FeedbackReadDTO> Replies { get; init; } = new();
    }
}