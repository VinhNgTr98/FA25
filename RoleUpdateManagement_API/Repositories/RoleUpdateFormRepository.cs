using Microsoft.EntityFrameworkCore;
using RoleUpdateManagement_API.Data;
using RoleUpdateManagement_API.Models;
using RoleUpdateManagement_API.Repositories.Interfaces;
using System;

namespace RoleUpdateManagement_API.Repositories
{
    public class RoleUpdateFormRepository : IRoleUpdateFormRepository
    {
        private readonly RoleUpdateFormContext _context;

        public RoleUpdateFormRepository(RoleUpdateFormContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleUpdateForm>> GetAllAsync()
        {
            return await _context.RoleUpdateForms.ToListAsync();
        }

        public async Task<RoleUpdateForm?> GetByIdAsync(Guid id)
        {
            return await _context.RoleUpdateForms.FindAsync(id);
        }

        public async Task AddAsync(RoleUpdateForm entity)
        {
            await _context.RoleUpdateForms.AddAsync(entity);
        }

        public async Task UpdateAsync(RoleUpdateForm entity)
        {
            _context.RoleUpdateForms.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.RoleUpdateForms.FindAsync(id);
            if (entity != null)
            {
                _context.RoleUpdateForms.Remove(entity);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
