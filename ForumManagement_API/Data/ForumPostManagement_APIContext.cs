using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ForumPostManagement_API.Models;

namespace ForumPostManagement_API.Data
{
    public class ForumPostManagement_APIContext : DbContext
    {
        public ForumPostManagement_APIContext (DbContextOptions<ForumPostManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<ForumPostManagement_API.Models.ForumPost> ForumPost { get; set; } = default!;
    }
}
