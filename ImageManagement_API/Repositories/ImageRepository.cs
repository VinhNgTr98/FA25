using ImageManagement_API.Data;
using ImageManagement_API.Models;
using ImageManagement_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImageManagement_API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly Image_APIContext _context;
        private readonly DbSet<Image> _dbSet;

        public ImageRepository(Image_APIContext context)
        {
            _context = context;
            _dbSet = context.Set<Image>();
        }

        public async Task<IEnumerable<Image>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(Image image)
        {
            await _dbSet.AddAsync(image);
        }

        public void Update(Image image)
        {
            _dbSet.Update(image);
        }

        public void Delete(Image image)
        {
            _dbSet.Remove(image);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
