using Microsoft.EntityFrameworkCore;
using TourAgencyManagement_API.Models;

namespace TourAgencyManagement_API.Data
{
    public class TourAgencyContext : DbContext
    {
        public TourAgencyContext(DbContextOptions<TourAgencyContext> options)
            : base(options)
        {
        }

        public DbSet<TourAgency> TourAgencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TourAgency>()
                        .Property(x => x.TourAgencyId)
                        .ValueGeneratedNever();

            
        }
    }
}
