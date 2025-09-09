using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartManagement_Api.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        [Required]
        public int CartID { get; set; }

        [Required]
        public CartItemType ItemType { get; set; }

        [Required]
        public Guid ItemID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("CartID")]
        public virtual Cart Cart { get; set; }
    }

    public enum CartItemType
    {
        Room,      
        Tour,     
        Vehicle,   
        Service    
    }
}