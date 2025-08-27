using Microsoft.EntityFrameworkCore;
using ImageManagement_API.Models;

namespace ImageManagement_API.Data
{
    public class Image_APIContext : DbContext
    {
        public Image_APIContext(DbContextOptions<Image_APIContext> options)
            : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
    }
}
