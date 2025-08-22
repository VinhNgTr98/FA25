using Categories_API.DTOs;

namespace Categories_API.Services
{
    public interface ICategoryService
    {
        // READ
        Task<IEnumerable<CategoryReadDto>> GetAllAsync();
        Task<CategoryReadDto?> GetByIdAsync(int id);

        // CREATE
        Task<CategoryReadDto> AddAsync(CategoryCreateDto categoryDto);

        // UPDATE
        Task<CategoryReadDto?> UpdateAsync(int id, CategoryUpdateDto categoryDto);

        // DELETE
        Task<bool> DeleteAsync(int id);
    }
}
