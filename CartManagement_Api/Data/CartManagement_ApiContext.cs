using Microsoft.EntityFrameworkCore;
using CartManagement_Api.Models;

namespace CartManagement_Api.Data
{
    public class CartManagement_ApiContext : DbContext
    {
        public CartManagement_ApiContext(DbContextOptions<CartManagement_ApiContext> options) : base(options) { }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        
    }
}