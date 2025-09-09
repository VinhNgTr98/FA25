using System.ComponentModel.DataAnnotations;

namespace CartManagement_Api.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int UserID { get; set; }   // chỉ lưu ID tham chiếu
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation nội bộ Cart service (ok vì cùng context)
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }



}
