using Microsoft.EntityFrameworkCore;
using VehicleManageMent_API.Models;

namespace VehicleManageMent_API.Data
{
    public class VehicleContext : DbContext
    {
        public VehicleContext(DbContextOptions<VehicleContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
