using ReportManagement_API.Models;

namespace ReportManagement_API.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report?> GetByIdAsync(int id);
        Task AddAsync(Report entity);
        Task UpdateAsync(Report entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
