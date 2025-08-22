using Categories_API.Models;
using Categories_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Categories_API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Categories_APIContext _context;

        public CategoryRepository(Categories_APIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Category.FindAsync(id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existing = await _context.Category.FindAsync(category.CategoryID);
            if (existing == null) return null;
            existing.CategoryType = category.CategoryType;
            existing.CategoryName = category.CategoryName;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return false;
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
