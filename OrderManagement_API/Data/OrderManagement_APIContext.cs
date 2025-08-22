using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagement_API.Models;

namespace OrderManagement_API.Data
{
    public class OrderManagement_APIContext : DbContext
    {
        public OrderManagement_APIContext (DbContextOptions<OrderManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<OrderManagement_API.Models.Order> Order { get; set; } = default!;
    }
}
