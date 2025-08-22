using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User_API.Models;

namespace Users_API.Data
{
    public class Users_APIContext : DbContext
    {
        public Users_APIContext (DbContextOptions<Users_APIContext> options)
            : base(options)
        {
        }

        public DbSet<User_API.Models.User> User { get; set; } = default!;
    }
}
