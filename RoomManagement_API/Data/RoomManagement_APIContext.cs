using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomManagement_API.Models;

namespace RoomManagement_API.Data
{
    public class RoomManagement_APIContext : DbContext
    {
        public RoomManagement_APIContext (DbContextOptions<RoomManagement_APIContext> options)
            : base(options)
        {
        }
        public DbSet<RoomManagement_API.Models.Room> Room { get; set; } = default!;

        
    }
}
