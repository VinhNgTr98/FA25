using AutoMapper;
using Categories_API.DTOs;
using Categories_API.Models;
using Categories_API.Repositories;

namespace Categories_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryReadDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryReadDto>>(entities);
        }

        public async Task<CategoryReadDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<CategoryReadDto>(entity);
        }

        public async Task<CategoryReadDto> AddAsync(CategoryCreateDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            var result = await _repo.AddAsync(entity);
            return _mapper.Map<CategoryReadDto>(result);
        }

        public async Task<CategoryReadDto?> UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing); // ID sẽ không bị map do Ignore()

            var result = await _repo.UpdateAsync(existing);
            return _mapper.Map<CategoryReadDto>(result);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
