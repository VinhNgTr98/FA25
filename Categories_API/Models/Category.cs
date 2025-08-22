using System.ComponentModel.DataAnnotations;

namespace Categories_API.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryType { get; set; }
        public string CategoryName { get; set; }
    }
}
