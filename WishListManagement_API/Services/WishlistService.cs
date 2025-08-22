using AutoMapper;
using WishListManagement_API.DTOs;
using WishListManagement_API.Models;
using WishListManagement_API.Repositories;

namespace WishListManagement_API.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _repo;
        private readonly IMapper _mapper;

        public WishlistService(IWishlistRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<WishlistDto> CreateAsync(CreateWishlistDto dto, CancellationToken ct = default)
        {
            // Chặn trùng theo (UserId, TargetType, TargetId)
            if (await _repo.ExistsAsync(dto.UserId, dto.TargetType, dto.TargetId, ct))
                throw new ArgumentException("Item đã tồn tại trong wishlist của người dùng.");

            var entity = _mapper.Map<Wishlist>(dto);
            var added = await _repo.AddAsync(entity, ct);
            return _mapper.Map<WishlistDto>(added);
        }

        public async Task<WishlistDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return entity is null ? null : _mapper.Map<WishlistDto>(entity);
        }

        public async Task<IReadOnlyList<WishlistDto>> GetByUserAsync(int userId, CancellationToken ct = default)
        {
            var list = await _repo.GetByUserAsync(userId, ct);
            return list.Select(_mapper.Map<WishlistDto>).ToList();
        }

        public async Task UpdateAsync(int id, UpdateWishlistDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException("Không tìm thấy wishlist.");

            // Check trùng với target mới cho cùng user
            if (await _repo.ExistsAsync(entity.UserId, dto.TargetType, dto.TargetId, ct))
                throw new ArgumentException("Item đã tồn tại trong wishlist của người dùng.");

            _mapper.Map(dto, entity);   // AutoMapper set đúng 1 cột raw theo TargetType
            await _repo.UpdateAsync(entity, ct);
        }

        public Task DeleteAsync(int id, CancellationToken ct = default)
            => _repo.DeleteAsync(id, ct);
    }
}
