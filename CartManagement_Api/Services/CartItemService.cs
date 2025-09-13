using AutoMapper;
using CartManagement_Api.Data;
using CartManagement_Api.DTOs;
using CartManagement_Api.Models;
using CartManagement_Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CartManagement_Api.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _repository;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemReadDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CartItemReadDto>>(items);
        }

        public async Task<CartItemReadDto?> GetByIdAsync(int cartItemId)
        {
            var item = await _repository.GetByIdAsync(cartItemId);
            return _mapper.Map<CartItemReadDto?>(item);
        }

        public async Task<CartItemReadDto> CreateAsync(CartItemCreateDto dto)
        {
            var item = _mapper.Map<CartItem>(dto);
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;

            var created = await _repository.AddAsync(item);
            return _mapper.Map<CartItemReadDto>(created);
        }

        public async Task<CartItemReadDto?> UpdateAsync(int cartItemId, CartItemUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(cartItemId);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<CartItemReadDto>(updated);
        }

        public async Task<bool> DeleteAsync(int cartItemId)
        {
            return await _repository.DeleteAsync(cartItemId);
        }
    }
}