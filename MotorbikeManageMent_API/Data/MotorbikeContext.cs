using Microsoft.EntityFrameworkCore;
using MotorbikeManageMent_API.Models;


namespace MotorbikeManageMent_API.Data
{
    public class MotorbikeContext : DbContext
    {
        public MotorbikeContext(DbContextOptions<MotorbikeContext> options)
            : base(options)
        {
        }

        public DbSet<Motorbike> Motorbike { get; set; }
    }
}
