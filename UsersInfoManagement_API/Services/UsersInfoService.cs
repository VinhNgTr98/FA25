using AutoMapper;
using UsersInfoManagement_API.Dtos.UsersInfo;
using UsersInfoManagement_API.Models;
using UsersInfoManagement_API.Repositories;

namespace UsersInfoManagement_API.Services
{
    public class UsersInfoService : IUsersInfoService
    {
        private readonly IUsersInfoRepository _repo;
        private readonly IMapper _mapper;

        public UsersInfoService(IUsersInfoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UsersInfoReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _repo.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<UsersInfoReadDto>>(list);
        }

        public async Task<UsersInfoReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<UsersInfoReadDto>(entity);
        }

        public async Task<UsersInfoReadDto?> GetByUserIdAsync(int usersId, CancellationToken ct = default)
        {
            var entity = await _repo.GetByUserIdAsync(usersId, ct);
            return entity is null ? null : _mapper.Map<UsersInfoReadDto>(entity);
        }

        public async Task<UsersInfoReadDto> CreateAsync(UsersInfoCreateDto dto, CancellationToken ct = default)
        {
            if (!await _repo.UserExistsAsync(dto.UsersID, ct))
                throw new InvalidOperationException("User không tồn tại");

            if (await _repo.GetByUserIdAsync(dto.UsersID, ct) is not null)
                throw new InvalidOperationException("User đã có UsersInfo");

            if (await _repo.EmailExistsAsync(dto.Email, null, ct))
                throw new InvalidOperationException("Email đã tồn tại");

            var entity = _mapper.Map<UsersInfo>(dto);
            entity = await _repo.AddAsync(entity, ct);
            return _mapper.Map<UsersInfoReadDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, UsersInfoUpdateDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity is null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var exists = await _repo.EmailExistsAsync(dto.Email, excludeId: id, ct);
                if (exists) throw new InvalidOperationException("Email đã tồn tại");
            }

            // (tuỳ chọn) không cho đổi UsersID
            if (dto.UsersID.HasValue && dto.UsersID.Value != entity.UsersID)
                throw new InvalidOperationException("Không cho phép đổi UsersID");

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity, ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity is null) return false;

            await _repo.DeleteAsync(entity, ct);
            return true;
        }
    }
}
