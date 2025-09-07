using System.ComponentModel.DataAnnotations;

namespace CouponManagement_API.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        // Không dùng ForeignKey, chỉ lưu ID
        public int CategoryID { get; set; }

        public string Name { get; set; } = string.Empty; // nvarchar(100), not null

        public decimal DiscountPercentage { get; set; } // decimal(5,2), CHECK (0–100)

        public bool IsActive { get; set; } = false; // bit, default 0

        public DateTime StartDate { get; set; } // not null

        public DateTime ExpireDate { get; set; } // not null

        public int? MaxUsage { get; set; } // nullable int

        public int UsedCount { get; set; } = 0; // default 0

        public decimal? MinOrderAmount { get; set; } // decimal(10,2), nullable
    }
}
