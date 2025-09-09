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

        public async Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            var orders = await _repo.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
        }

        public async Task<OrderReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var order = await _repo.GetByIdAsync(id, ct);
            return order == null ? null : _mapper.Map<OrderReadDto>(order);
        }

        public async Task<OrderReadDto> CreateAsync(OrderCreateDto dto, CancellationToken ct = default)
        {
            var order = _mapper.Map<Order>(dto);
            order.OrderDate = DateTime.UtcNow;
            var created = await _repo.AddAsync(order, ct);
            return _mapper.Map<OrderReadDto>(created);
        }

        public async Task<bool> UpdateAsync(int id, OrderUpdateDto dto, CancellationToken ct = default)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            return await _repo.UpdateAsync(existing, ct);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
            => _repo.DeleteAsync(id, ct);
    }
}
