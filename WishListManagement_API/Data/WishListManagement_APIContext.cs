using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WishListManagement_API.Models;

namespace WishListManagement_API.Data
{
    public class WishListManagement_APIContext : DbContext
    {
        public WishListManagement_APIContext (DbContextOptions<WishListManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<WishListManagement_API.Models.Wishlist> Wishlist { get; set; } = default!;
    }
}
