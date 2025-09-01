using Microsoft.EntityFrameworkCore;
using VehicleAgencyManagement_API.Models;

namespace VehicleAgencyManagement_API.Data
{
    public class VehicleAgencyContext : DbContext
    {
        public VehicleAgencyContext(DbContextOptions<VehicleAgencyContext> options)
            : base(options)
        {
        }

        public DbSet<VehicleAgency> VehicleAgencies { get; set; }
    }
}
