using Microsoft.EntityFrameworkCore;
using OrderManagement_API.Models;

namespace OrderManagement_API.Data
{
    public class BookingManagement_APIContext : DbContext
    {
        public BookingManagement_APIContext(DbContextOptions<BookingManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Orders { get; set; } = default!;
        public DbSet<BookingItem> OrderItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(e =>
            {
                e.Property(x => x.Status).HasMaxLength(50).IsRequired().HasDefaultValue("Pending");
                e.HasIndex(x => x.UserID);
                e.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
                e.Property(x => x.TaxAmount).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<BookingItem>(e =>
            {
                e.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
                e.HasIndex(x => x.BookingId);
                e.HasOne(x => x.Booking)
                 .WithMany(o => o.Items)
                 .HasForeignKey(x => x.BookingId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}