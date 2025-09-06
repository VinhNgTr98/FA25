using CouponManagement_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CouponManagement_API.Data
{
    public class CouponContext : DbContext
    {
        public CouponContext(DbContextOptions<CouponContext> options)
            : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }
    }
}
