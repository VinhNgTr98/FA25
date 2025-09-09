using Microsoft.EntityFrameworkCore;
using OrderManagement_API.Models;

namespace OrderManagement_API.Data
{
        public class OrderManagement_APIContext : DbContext
        {
            public OrderManagement_APIContext(DbContextOptions<OrderManagement_APIContext> options) : base(options) { }

            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderItem> OrderItems { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Order>()
                    .HasMany(o => o.Items)
                    .WithOne(i => i.Order)
                    .HasForeignKey(i => i.OrderID)
                    .OnDelete(DeleteBehavior.Cascade);

                base.OnModelCreating(modelBuilder);
            }
        }
}
