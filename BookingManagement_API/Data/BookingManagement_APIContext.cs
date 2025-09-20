using Microsoft.EntityFrameworkCore;
using BookingManagement_API.Models;

namespace BookingManagement_API.Data
{
    public class BookingManagement_APIContext : DbContext
    {
        public BookingManagement_APIContext(DbContextOptions<BookingManagement_APIContext> options)
            : base(options)
        {
        }

        // Giữ nguyên DbSet tên "Booking" cho phù hợp với BookingsController scaffold
        public DbSet<Booking> Booking { get; set; } = default!;

        // Thêm DbSet cho BookingItem (đúng kiểu)
        public DbSet<BookingItem> BookingItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Đặt tên bảng rõ ràng để tránh lệ thuộc vào quy ước
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<BookingItem>().ToTable("BookingItems");

            modelBuilder.Entity<Booking>()
                .HasMany(b => b.Items)
                .WithOne(i => i.Booking!)
                .HasForeignKey(i => i.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index phụ trợ (không bắt buộc)
            modelBuilder.Entity<BookingItem>()
                .HasIndex(i => i.BookingId);
        }
    }
}