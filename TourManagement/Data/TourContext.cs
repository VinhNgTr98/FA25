using Microsoft.EntityFrameworkCore;
using TourManagement.Model;

namespace TourManagement.Data
{
    public class TourContext : DbContext
    {
        public TourContext(DbContextOptions<TourContext> options)
            : base(options)
        {
        }

        public DbSet<Tour> Tours { get; set; }
    }
}
