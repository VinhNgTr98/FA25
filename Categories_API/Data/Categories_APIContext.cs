

using Microsoft.EntityFrameworkCore;

namespace Categories_API.Data
{
    public class Categories_APIContext : DbContext
    {
        public Categories_APIContext (DbContextOptions<Categories_APIContext> options)
            : base(options)
        {
        }

        public DbSet<Categories_API.Models.Category> Category { get; set; } = default!;
    }
}
