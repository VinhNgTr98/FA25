using Microsoft.EntityFrameworkCore;
using UserCouponManagement_API.Models;

namespace UserCouponManagement_API.Data
{
    public class UserCouponContext : DbContext
    {
        public UserCouponContext(DbContextOptions<UserCouponContext> options)
            : base(options)
        {
        }

        public DbSet<UserCoupon> UserCoupons { get; set; }
    }
}
