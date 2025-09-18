using Microsoft.EntityFrameworkCore;
using OrderManagement_API.Models;

namespace OrderManagement_API.Data
{
    public class OrderManagement_APIContext : DbContext
    {
        public OrderManagement_APIContext(DbContextOptions<OrderManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderItem> OrderItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(e =>
            {
                e.Property(x => x.Status).HasMaxLength(50).IsRequired().HasDefaultValue("Pending");
                e.HasIndex(x => x.UserID);
                e.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
                e.Property(x => x.TaxAmount).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<OrderItem>(e =>
            {
                e.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
                e.HasIndex(x => x.OrderID);
                e.HasOne(x => x.Order)
                 .WithMany(o => o.Items)
                 .HasForeignKey(x => x.OrderID)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}