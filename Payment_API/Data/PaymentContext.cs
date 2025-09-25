using Microsoft.EntityFrameworkCore;
using Payment_API.Models;

namespace Payment_API.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }

        public DbSet<Payment> Payments { get; set; }
    }
}
