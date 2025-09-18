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
            order.Status = "Pending";
            order.OrderDate = DateTime.UtcNow;
            order.TotalAmount = (order.TaxAmount ?? 0m); 

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

        public async Task<IEnumerable<OrderReadDto>> GetByUserIdAsync(int userId, CancellationToken ct = default)
        {
            var orders = await _repo.GetByUserIdAsync(userId, ct);
            return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
        }

        public async Task<IEnumerable<OrderItemReadDto>> GetItemsByOrderIdAsync(int orderId, CancellationToken ct = default)
        {
            var items = await _repo.GetItemsByOrderIdAsync(orderId, ct);
            return _mapper.Map<IEnumerable<OrderItemReadDto>>(items);
        }

        public async Task<OrderItemReadDto?> AddItemAsync(int orderId, OrderItemCreateDto dto, CancellationToken ct = default)
        {
            // Tìm order trước (để trả NotFound nếu không có và để tính lại total)
            var order = await _repo.GetByIdAsync(orderId, ct);
            if (order == null) return null;

            // Tạo item
            var entity = _mapper.Map<OrderItem>(dto);
            entity.OrderID = orderId;
            var created = await _repo.AddItemAsync(entity, ct);

            // Tính lại tổng tiền của Order = sum(items) + tax
            var items = await _repo.GetItemsByOrderIdAsync(orderId, ct);
            var itemsTotal = items.Sum(i => i.Quantity * i.UnitPrice);
            var tax = order.TaxAmount ?? 0m;
            order.TotalAmount = Math.Round(itemsTotal + tax, 2, MidpointRounding.AwayFromZero);
            await _repo.UpdateAsync(order, ct);

            return _mapper.Map<OrderItemReadDto>(created);
        }
    }
}