using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarManagement_API.Models;

namespace CarManagement_API.Data
{
    public class CarManagement_APIContext : DbContext
    {
        public CarManagement_APIContext (DbContextOptions<CarManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<CarManagement_API.Models.Car> Car { get; set; } = default!;
    }
}
