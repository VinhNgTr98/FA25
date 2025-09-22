using System.ComponentModel.DataAnnotations;

namespace FeedbackManagement_API.Model
{
    public class Feedbacks
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        public int UserID { get; set; } 

        [Required]
        public Guid LinkedId { get; set; }  // Đối tượng được feedback

        [Required, StringLength(30)]
        public string LinkedType { get; set; } = default!; // 'room', 'vehicle', 'tour', ...

        public bool IsReply { get; set; } = false;

        public int? ReplyId { get; set; } // FeedbackId mà feedback này reply

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        public Feedbacks? ReplyTo { get; set; }
        public ICollection<Feedbacks> Replies { get; set; } = new List<Feedbacks>();
    }
}