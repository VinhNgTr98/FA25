using ImageManagement_API.Models;

namespace ImageManagement_API.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetAllAsync();
        Task<Image?> GetByIdAsync(int id);
        Task AddAsync(Image image);
        void Update(Image image);
        void Delete(Image image);
        Task SaveChangesAsync();
    }
}
