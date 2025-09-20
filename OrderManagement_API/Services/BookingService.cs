using AutoMapper;
using OrderManagement_API.DTOs;
using OrderManagement_API.Models;
using OrderManagement_API.Repositories;

namespace OrderManagement_API.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            var orders = await _repo.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<BookingReadDto>>(orders);
        }

        public async Task<BookingReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var order = await _repo.GetByIdAsync(id, ct);
            return order == null ? null : _mapper.Map<BookingReadDto>(order);
        }

        public async Task<BookingReadDto> CreateAsync(BookingCreateDto dto, CancellationToken ct = default)
        {
            var order = _mapper.Map<Booking>(dto);
            order.Status = "Pending";
            order.BookingDate = DateTime.UtcNow;
            order.TotalAmount = (order.TaxAmount ?? 0m); 

            var created = await _repo.AddAsync(order, ct);
            return _mapper.Map<BookingReadDto>(created);
        }

        public async Task<bool> UpdateAsync(int id, BookingUpdateDto dto, CancellationToken ct = default)
        {
            var existing = await _repo.GetByIdAsync(id, ct);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            return await _repo.UpdateAsync(existing, ct);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
            => _repo.DeleteAsync(id, ct);

        public async Task<IEnumerable<BookingReadDto>> GetByUserIdAsync(int userId, CancellationToken ct = default)
        {
            var orders = await _repo.GetByUserIdAsync(userId, ct);
            return _mapper.Map<IEnumerable<BookingReadDto>>(orders);
        }

        public async Task<IEnumerable<BookingItemReadDto>> GetItemsByOrderIdAsync(int orderId, CancellationToken ct = default)
        {
            var items = await _repo.GetItemsByOrderIdAsync(orderId, ct);
            return _mapper.Map<IEnumerable<BookingItemReadDto>>(items);
        }

        public async Task<BookingItemReadDto?> AddItemAsync(int orderId, BookingItemCreateDto dto, CancellationToken ct = default)
        {
            // Tìm order trước (để trả NotFound nếu không có và để tính lại total)
            var order = await _repo.GetByIdAsync(orderId, ct);
            if (order == null) return null;

            // Tạo item
            var entity = _mapper.Map<BookingItem>(dto);
            entity.BookingId = orderId;
            var created = await _repo.AddItemAsync(entity, ct);

            // Tính lại tổng tiền của Order = sum(items) + tax
            var items = await _repo.GetItemsByOrderIdAsync(orderId, ct);
            var itemsTotal = items.Sum(i => i.Quantity * i.UnitPrice);
            var tax = order.TaxAmount ?? 0m;
            order.TotalAmount = Math.Round(itemsTotal + tax, 2, MidpointRounding.AwayFromZero);
            await _repo.UpdateAsync(order, ct);

            return _mapper.Map<BookingItemReadDto>(created);
        }
    }
}