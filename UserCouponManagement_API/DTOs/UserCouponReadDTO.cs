using System.ComponentModel.DataAnnotations;

namespace UserCouponManagement_API.DTOs
{
    public class UserCouponReadDTO
    {
        [Key]
        public int UserCouponId { get; set; }
        public int UserId { get; set; }
        public int CouponId { get; set; }
        public DateTime? UseAt { get; set; }
    }
}
