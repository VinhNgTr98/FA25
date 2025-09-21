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
        public DbSet<Itinerary> Itineraries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Itinerary>()
                .HasOne(i => i.Tour)
                .WithMany(t => t.Itineraries)
                .HasForeignKey(i => i.TourID)
                .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Tour thì xóa luôn Itineraries
        }
    }
}
