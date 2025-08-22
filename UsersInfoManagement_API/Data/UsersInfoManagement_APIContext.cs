using Microsoft.EntityFrameworkCore;
using UsersInfoManagement_API.Models;

namespace UsersInfoManagement_API.Data
{
    public class UsersInfoManagement_APIContext : DbContext
    {
        public UsersInfoManagement_APIContext(DbContextOptions<UsersInfoManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<UsersInfo> UsersInfo { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!; // map tới bảng Users có sẵn

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bảng Users đã tồn tại trong DB → KHÔNG cho migrations đụng vào
            modelBuilder
                .Entity<User>()
                .ToTable("Users", tb => tb.ExcludeFromMigrations())
                .HasKey(u => u.UserID);

            // UsersInfo cấu hình
            modelBuilder.Entity<UsersInfo>(e =>
            {
                e.ToTable("UsersInfo");

                e.Property(x => x.FullName).HasMaxLength(100).IsRequired();
                e.Property(x => x.ProfilePictureUrl).HasMaxLength(255);
                e.Property(x => x.Email).HasMaxLength(100).IsRequired();
                e.Property(x => x.PhoneNumber).HasMaxLength(15);
                e.Property(x => x.Address).HasMaxLength(255);

                e.HasIndex(x => x.Email).IsUnique();
                e.HasIndex(x => x.UsersID).IsUnique(); // 1–1: mỗi UsersID chỉ có 1 UsersInfo

                e.HasOne(x => x.User)
                 .WithOne(u => u.UsersInfo)
                 .HasForeignKey<UsersInfo>(x => x.UsersID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
