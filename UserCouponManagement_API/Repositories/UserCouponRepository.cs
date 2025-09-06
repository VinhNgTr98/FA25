using Microsoft.EntityFrameworkCore;
using System;
using UserCouponManagement_API.Data;
using UserCouponManagement_API.Models;
using UserCouponManagement_API.Repositories.Interfaces;

namespace UserCouponManagement_API.Repositories
{
    public class UserCouponRepository : IUserCouponRepository
    {
        private readonly UserCouponContext _context;

        public UserCouponRepository(UserCouponContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserCoupon>> GetAllAsync()
        {
            return await _context.UserCoupons.ToListAsync();
        }

        public async Task<UserCoupon?> GetByIdAsync(int id)
        {
            return await _context.UserCoupons.FindAsync(id);
        }

        public async Task AddAsync(UserCoupon entity)
        {
            await _context.UserCoupons.AddAsync(entity);
        }

        public async Task UpdateAsync(UserCoupon entity)
        {
            _context.UserCoupons.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var userCoupon = await _context.UserCoupons.FindAsync(id);
            if (userCoupon != null)
            {
                _context.UserCoupons.Remove(userCoupon);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
