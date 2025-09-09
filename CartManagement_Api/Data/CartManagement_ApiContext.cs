using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CartManagement_Api.Models;

namespace CartManagement_Api.Data
{
    public class CartManagement_ApiContext : DbContext
    {
        public CartManagement_ApiContext(DbContextOptions<CartManagement_ApiContext> options) : base(options) { }

        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.CartID);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.Cart)
                .HasForeignKey(i => i.CartID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasKey(i => i.CartItemID);

            // Enum -> string max 50 (Room/Tour/Vehicle/Service)
            modelBuilder.Entity<CartItem>()
                .Property(i => i.ItemType)
                .HasConversion<string>()
                .HasMaxLength(50);

            // Tránh trùng một item "cùng khóa logic" trong 1 cart:
            modelBuilder.Entity<CartItem>()
                .HasIndex(i => new { i.CartID, i.ItemType, i.ItemID, i.StartDate, i.EndDate })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
