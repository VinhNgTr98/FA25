using ReportManagement_API.DTOs;

namespace ReportManagement_API.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportReadDTO>> GetAllAsync();
        Task<ReportReadDTO?> GetByIdAsync(int id);
        Task<ReportReadDTO> CreateAsync(ReportCreateDTO dto);
        Task<bool> UpdateAsync(int id, ReportUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
