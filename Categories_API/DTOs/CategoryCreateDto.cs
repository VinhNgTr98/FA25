using System.ComponentModel.DataAnnotations;

namespace Categories_API.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        public string CategoryType { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
