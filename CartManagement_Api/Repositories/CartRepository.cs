using CartManagement_Api.Data;
using CartManagement_Api.Models;
using CartManagement_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace CartManagement_Api.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartManagement_ApiContext _context;

        public CartRepository(CartManagement_ApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(int cartId)
        {
            return await _context.Carts.FindAsync(cartId);
        }

        public async Task<Cart> AddAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> DeleteAsync(int cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null) return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}