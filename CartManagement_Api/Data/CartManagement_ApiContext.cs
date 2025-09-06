using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CartManagement_Api.Models;

namespace CartManagement_Api.Data
{
    public class CartManagement_ApiContext : DbContext
    {
        public CartManagement_ApiContext (DbContextOptions<CartManagement_ApiContext> options)
            : base(options)
        {
        }

        public DbSet<CartManagement_Api.Models.Cart> Cart { get; set; } = default!;
    }
}
