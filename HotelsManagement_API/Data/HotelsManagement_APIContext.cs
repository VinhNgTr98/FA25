using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelsManagement_API.Models;

namespace HotelsManagement_API.Data
{
    public class HotelsManagement_APIContext : DbContext
    {
        public HotelsManagement_APIContext (DbContextOptions<HotelsManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<HotelsManagement_API.Models.Hotel> Hotel { get; set; } = default!;
    }
}
