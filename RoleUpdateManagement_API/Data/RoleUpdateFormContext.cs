using Microsoft.EntityFrameworkCore;
using RoleUpdateManagement_API.Models;

namespace RoleUpdateManagement_API.Data
{
    public class RoleUpdateFormContext : DbContext
    {
        public RoleUpdateFormContext(DbContextOptions<RoleUpdateFormContext> options)
            : base(options)
        {
        }

        public DbSet<RoleUpdateForm> RoleUpdateForms { get; set; }
    }
}