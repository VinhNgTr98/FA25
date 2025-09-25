using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ForumPostManagement_API.DTOs
{
    public class ChangeApprovalStatusDto
    {
        // Chỉ nhận "Approved" hoặc "Rejected"
        [Required, MaxLength(20)]
        [RegularExpression("^(Approved|Rejected)$")]
        [DefaultValue("Approved")]
        public string ApprovalStatus { get; set; } = null!;

        // Bắt buộc khi Status = "Rejected"; khi "Approved" có thể null
        [MaxLength(400)]
        public string? Note { get; set; }
    }
}
