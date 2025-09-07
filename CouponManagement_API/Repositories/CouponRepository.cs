using CouponManagement_API.Data;
using CouponManagement_API.Models;
using CouponManagement_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace CouponManagement_API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly CouponContext _context;

        public CouponRepository(CouponContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task<Coupon?> GetByIdAsync(int id)
        {
            return await _context.Coupons.FindAsync(id);
        }

        public async Task AddAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
        }

        public async Task UpdateAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon != null)
            {
                _context.Coupons.Remove(coupon);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
