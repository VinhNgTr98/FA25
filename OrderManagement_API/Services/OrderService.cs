using AutoMapper;
using OrderManagement_API.DTOs;
using OrderManagement_API.Models;
using OrderManagement_API.Repositories;

namespace OrderManagement_API.Services
{
    
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderReadDto>> GetAllAsync()
        {
            var orders = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
        }

        public async Task<OrderReadDto?> GetByIdAsync(int id)
        {
            var order = await _repo.GetByIdAsync(id);
            return order == null ? null : _mapper.Map<OrderReadDto>(order);
        }

        public async Task<OrderReadDto> AddAsync(OrderCreateDto dto)
        {
            var order = _mapper.Map<Order>(dto);
            var created = await _repo.AddAsync(order);
            return _mapper.Map<OrderReadDto>(created);
        }

        public async Task<OrderReadDto?> UpdateAsync(int id, OrderUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            var updated = await _repo.UpdateAsync(existing);
            return updated == null ? null : _mapper.Map<OrderReadDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
