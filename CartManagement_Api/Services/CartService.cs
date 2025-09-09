using CartManagement_Api.DTOs;
using CartManagement_Api.Models;
using CartManagement_Api.Repositories;

namespace CartManagement_Api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        public CartService(ICartRepository repo) => _repo = repo;

        public async Task<CartReadDto> GetOrCreateAsync(int userId, CancellationToken ct = default)
        {
            var cart = await _repo.GetByUserIdAsync(userId, ct) ?? await _repo.CreateAsync(userId, ct);
            return Map(cart);
        }

        public async Task<CartItemReadDto> AddOrIncreaseAsync(int userId, CartItemCreateDto dto, CancellationToken ct = default)
        {
            var cart = await _repo.GetByUserIdAsync(userId, ct) ?? await _repo.CreateAsync(userId, ct);

            var existing = await _repo.FindItemAsync(cart.CartID, dto.ItemType, dto.ItemID, dto.StartDate, dto.EndDate, ct);
            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                existing.UpdatedAt = DateTime.UtcNow;
                await _repo.SaveChangesAsync(ct);
                return Map(existing);
            }

            var item = new CartItem
            {
                CartID = cart.CartID,
                ItemType = dto.ItemType,
                ItemID = dto.ItemID,
                Quantity = dto.Quantity,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
            await _repo.AddItemAsync(item, ct);
            return Map(item);
        }

        public async Task<bool> UpdateItemAsync(int userId, int cartItemId, CartItemUpdateDto dto, CancellationToken ct = default)
        {
            var cart = await _repo.GetByUserIdAsync(userId, ct);
            if (cart == null) return false;

            var item = await _repo.GetItemByIdAsync(cartItemId, ct);
            if (item == null || item.CartID != cart.CartID) return false;

            item.Quantity = dto.Quantity;
            item.StartDate = dto.StartDate;
            item.EndDate = dto.EndDate;
            item.UpdatedAt = DateTime.UtcNow;

            await _repo.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> RemoveItemAsync(int userId, int cartItemId, CancellationToken ct = default)
        {
            var cart = await _repo.GetByUserIdAsync(userId, ct);
            if (cart == null) return false;
            return await _repo.RemoveItemAsync(cart.CartID, cartItemId, ct);
        }

        public async Task<bool> ClearAsync(int userId, CancellationToken ct = default)
        {
            var cart = await _repo.GetByUserIdAsync(userId, ct);
            if (cart == null) return false;
            return await _repo.ClearAsync(cart.CartID, ct);
        }

        // -------- Mapping helpers --------
        private static CartReadDto Map(Cart c) => new CartReadDto
        {
            CartID = c.CartID,
            UserID = c.UserID,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            Items = c.Items.Select(Map).ToList()
        };

        private static CartItemReadDto Map(CartItem i) => new CartItemReadDto
        {
            CartItemID = i.CartItemID,
            CartID = i.CartID,
            ItemType = i.ItemType,
            ItemID = i.ItemID,
            Quantity = i.Quantity,
            StartDate = i.StartDate,
            EndDate = i.EndDate
        };
    }
}
