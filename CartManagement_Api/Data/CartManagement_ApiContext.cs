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
            modelBuilder.Entity<Cart>(e =>
            {
                e.HasKey(c => c.CartID);
                e.Property(c => c.RowVersion).IsRowVersion();
                e.HasMany(c => c.Items)
                 .WithOne(i => i.Cart)
                 .HasForeignKey(i => i.CartID)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(c => c.UserID);
            });

            modelBuilder.Entity<CartItem>(e =>
            {
                e.HasKey(i => i.CartItemID);
                e.Property(i => i.ItemType).HasMaxLength(50).IsRequired();
                e.Property(i => i.RowVersion).IsRowVersion();

                // Unique logic key (MySQL: NULL khác nhau -> chấp nhận vì Service merge. Nếu muốn chặt hơn dùng COALESCE raw SQL).
                e.HasIndex(i => new { i.CartID, i.ItemType, i.ItemID, i.StartDate, i.EndDate })
                 .IsUnique()
                 .HasDatabaseName("UX_CartItems_Key");

                e.HasCheckConstraint("CK_CartItems_ItemType",
                    "ItemType IN ('Room','Tour','Vehicle','Service')");
            });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            TouchTimestamps();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TouchTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void TouchTimestamps()
        {
            var now = DateTime.UtcNow;
            foreach (var e in ChangeTracker.Entries())
            {
                if (e.Entity is Cart c && e.State == EntityState.Modified)
                    c.UpdatedAt = now;
                if (e.Entity is CartItem ci && e.State == EntityState.Modified)
                    ci.UpdatedAt = now;
            }
        }
    }
}