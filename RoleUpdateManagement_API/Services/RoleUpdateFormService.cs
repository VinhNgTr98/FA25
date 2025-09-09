using AutoMapper;
using RoleUpdateManagement_API.DTOs;
using RoleUpdateManagement_API.Models;
using RoleUpdateManagement_API.Repositories.Interfaces;
using RoleUpdateManagement_API.Services.Interfaces;

namespace RoleUpdateManagement_API.Service
{
    public class RoleUpdateFormService : IRoleUpdateFormService
    {
        private readonly IRoleUpdateFormRepository _repo;
        private readonly IMapper _mapper;

        public RoleUpdateFormService(IRoleUpdateFormRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleUpdateFormReadDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleUpdateFormReadDTO>>(list);
        }

        public async Task<RoleUpdateFormReadDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<RoleUpdateFormReadDTO>(entity);
        }

        public async Task<RoleUpdateFormReadDTO> CreateAsync(RoleUpdateFormCreateDTO dto)
        {
            var entity = _mapper.Map<RoleUpdateForm>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<RoleUpdateFormReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, RoleUpdateFormUpdateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
