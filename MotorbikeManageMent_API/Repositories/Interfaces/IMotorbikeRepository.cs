using MotorbikeManageMent_API.DTOs;
using MotorbikeManageMent_API.Models;

namespace MotorbikeManageMent_API.Repositories.Interfaces
{
    public interface IMotorbikeRepository
    {
        Task<IEnumerable<Motorbike>> GetAllAsync();
        Task<Motorbike?> GetByIdAsync(Guid id);
        Task AddAsync(Motorbike vehicle);
        Task UpdateAsync(Motorbike vehicle);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();

        // Phương thức lọc
        Task<List<Motorbike>> GetFilteredMotorbikesAsync(
            string? transmission = null,
            string? fuel = null,
            string? motorbikeBrand = null,
            string? motorbikeName = null,
            int? engineCc = null
        );
    }
}
