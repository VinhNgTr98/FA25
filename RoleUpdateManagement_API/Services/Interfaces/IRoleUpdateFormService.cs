using RoleUpdateManagement_API.DTOs;

namespace RoleUpdateManagement_API.Services.Interfaces
{
    public interface IRoleUpdateFormService
    {
        Task<IEnumerable<RoleUpdateFormReadDTO>> GetAllAsync();
        Task<RoleUpdateFormReadDTO?> GetByIdAsync(Guid id);
        Task<RoleUpdateFormReadDTO> CreateAsync(RoleUpdateFormCreateDTO dto);
        Task<bool> UpdateAsync(Guid id, RoleUpdateFormUpdateDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
