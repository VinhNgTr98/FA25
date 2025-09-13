using AutoMapper;
using CartManagement_Api.Data;
using CartManagement_Api.DTOs;
using CartManagement_Api.Models;
using CartManagement_Api.Repositories.Interfaces;
using CartManagement_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CartManagement_Api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartReadDto>> GetAllAsync()
        {
            var carts = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CartReadDto>>(carts);
        }

        public async Task<CartReadDto?> GetByIdAsync(int cartId)
        {
            var cart = await _repository.GetByIdAsync(cartId);
            return _mapper.Map<CartReadDto?>(cart);
        }

        public async Task<CartReadDto> CreateAsync(CartCreateDto dto)
        {
            var cart = _mapper.Map<Cart>(dto);
            cart.CreatedAt = DateTime.UtcNow;
            cart.UpdatedAt = DateTime.UtcNow;

            var created = await _repository.AddAsync(cart);
            return _mapper.Map<CartReadDto>(created);
        }

        public async Task<CartReadDto?> UpdateAsync(int cartId, CartUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(cartId);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<CartReadDto>(updated);
        }

        public async Task<bool> DeleteAsync(int cartId)
        {
            return await _repository.DeleteAsync(cartId);
        }
    }
}