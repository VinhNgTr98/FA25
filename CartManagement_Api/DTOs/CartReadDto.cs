using System.ComponentModel.DataAnnotations;

namespace CartManagement_Api.DTOs
{
    public class CartReadDto
    {
        [Key]
        public int CartID { get; set; }
        public int UserID { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
    }
}