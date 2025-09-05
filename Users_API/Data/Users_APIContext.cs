using Microsoft.EntityFrameworkCore;
using User_API.Models;

namespace Users_API.Data
{
    public class Users_APIContext : DbContext
    {
        public Users_APIContext(DbContextOptions<Users_APIContext> options)
            : base(options) { }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<UsersInfo> UsersInfo { get; set; } = default!; // NEW

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1-1 Users <-> UsersInfo (UsersInfo.UserID là FK và cũng là unique)
            modelBuilder.Entity<User>()
                .HasOne(u => u.UsersInfo)
                .WithOne(ui => ui.User)
                .HasForeignKey<UsersInfo>(ui => ui.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
