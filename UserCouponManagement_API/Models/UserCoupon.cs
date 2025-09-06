using System.ComponentModel.DataAnnotations;

namespace UserCouponManagement_API.Models
{
    public class UserCoupon
    {
        [Key]
        public int UserCouponId { get; set; }

        // Foreign Key (chỉ lưu ID, không navigation)
        public int UserId { get; set; }

        public int CouponId { get; set; }

        public DateTime? UseAt { get; set; }
    }
}

