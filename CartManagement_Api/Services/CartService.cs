using AutoMapper;
using CartManagement_Api.Data;
using CartManagement_Api.DTOs;
using CartManagement_Api.Models;
using CartManagement_Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CartManagement_Api.Services
{
    public class CartService : ICartService
    {
        private readonly CartManagement_ApiContext _ctx;
        private readonly ICartRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        private static readonly HashSet<string> ValidItemTypes =
            new(new[] { "Room", "Tour", "Vehicle", "Service" }, StringComparer.OrdinalIgnoreCase);

        public CartService(CartManagement_ApiContext ctx, ICartRepository repo, IMapper mapper, ILogger<CartService> logger)
        {
            _ctx = ctx;
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CartReadDto> GetOrCreateCartAsync(int userId, CancellationToken ct = default)
        {
            var cart = await _repo.GetCartWithItemsByUserAsync(userId, ct);
            if (cart == null)
            {
                cart = new Cart { UserID = userId };
                await _repo.AddCartAsync(cart, ct);
            }
            return _mapper.Map<CartReadDto>(cart);
        }

        public async Task<CartReadDto?> GetCartByUserAsync(int userId, CancellationToken ct = default)
        {
            var cart = await _repo.GetCartWithItemsByUserAsync(userId, ct);
            return cart == null ? null : _mapper.Map<CartReadDto>(cart);
        }

        public async Task<CartReadDto> AddItemAsync(int userId, CartItemCreateDto dto, CancellationToken ct = default)
        {
            Validate(dto.ItemType, dto.Quantity, dto.StartDate, dto.EndDate);

            await using var tx = await _ctx.Database.BeginTransactionAsync(ct);

            var cart = await _repo.GetCartWithItemsByUserAsync(userId, ct);
            if (cart == null)
            {
                cart = new Cart { UserID = userId };
                _ctx.Carts.Add(cart);
            }

            var existing = cart.Items.FirstOrDefault(i =>
                i.ItemType.Equals(dto.ItemType, StringComparison.OrdinalIgnoreCase) &&
                i.ItemID == dto.ItemID &&
                i.StartDate == dto.StartDate &&
                i.EndDate == dto.EndDate);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var entity = _mapper.Map<CartItem>(dto);
                entity.ItemType = Normalize(dto.ItemType);
                cart.Items.Add(entity);
            }

            cart.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _ctx.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "AddItem failed");
                await tx.RollbackAsync(ct);
                throw;
            }

            return _mapper.Map<CartReadDto>(cart);
        }

        public async Task<CartReadDto> UpdateItemAsync(int userId, int cartItemId, CartItemUpdateDto dto, CancellationToken ct = default)
        {
            if (dto.RowVersion is null)
                throw new ArgumentException("RowVersion is required.");

            var clientVersion = Convert.FromBase64String(dto.RowVersion);

            await using var tx = await _ctx.Database.BeginTransactionAsync(ct);

            var entity = await _repo.GetCartItemByIdAsync(cartItemId, ct);
            if (entity == null || entity.Cart.UserID != userId)
                throw new KeyNotFoundException("CartItem not found.");

            if (!clientVersion.SequenceEqual(entity.RowVersion ?? Array.Empty<byte>()))
                throw new DbUpdateConcurrencyException("RowVersion mismatch.");

            if (dto.Quantity.HasValue)
            {
                if (dto.Quantity.Value <= 0)
                    throw new ArgumentException("Quantity must be > 0.");
                entity.Quantity = dto.Quantity.Value;
            }

            var newStart = dto.StartDate ?? entity.StartDate;
            var newEnd = dto.EndDate ?? entity.EndDate;

            if (dto.StartDate.HasValue || dto.EndDate.HasValue)
            {
                Validate(entity.ItemType, entity.Quantity, newStart, newEnd);
                entity.StartDate = newStart;
                entity.EndDate = newEnd;
            }

            // Merge nếu key mới trùng
            var cart = entity.Cart;
            var duplicate = cart.Items.FirstOrDefault(i =>
                i.CartItemID != entity.CartItemID &&
                i.ItemType.Equals(entity.ItemType, StringComparison.OrdinalIgnoreCase) &&
                i.ItemID == entity.ItemID &&
                i.StartDate == entity.StartDate &&
                i.EndDate == entity.EndDate);

            if (duplicate != null)
            {
                duplicate.Quantity += entity.Quantity;
                _ctx.CartItems.Remove(entity);
            }
            else
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }

            cart.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _ctx.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict updating CartItem {Id}", cartItemId);
                await tx.RollbackAsync(ct);
                throw;
            }

            return _mapper.Map<CartReadDto>(cart);
        }

        public async Task<bool> RemoveItemAsync(int userId, int cartItemId, string rowVersion, CancellationToken ct = default)
        {
            var clientVersion = Convert.FromBase64String(rowVersion);

            await using var tx = await _ctx.Database.BeginTransactionAsync(ct);

            var entity = await _repo.GetCartItemByIdAsync(cartItemId, ct);
            if (entity == null || entity.Cart.UserID != userId)
                return false;

            if (!clientVersion.SequenceEqual(entity.RowVersion ?? Array.Empty<byte>()))
                throw new DbUpdateConcurrencyException("RowVersion mismatch.");

            _ctx.CartItems.Remove(entity);
            entity.Cart.UpdatedAt = DateTime.UtcNow;

            await _ctx.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return true;
        }

        public async Task<bool> ClearCartAsync(int userId, CancellationToken ct = default)
        {
            await using var tx = await _ctx.Database.BeginTransactionAsync(ct);
            var cart = await _repo.GetCartWithItemsByUserAsync(userId, ct);
            if (cart == null) return false;

            _ctx.CartItems.RemoveRange(cart.Items);
            cart.UpdatedAt = DateTime.UtcNow;

            await _ctx.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return true;
        }

        public async Task<CartSummaryDto> GetSummaryAsync(int userId, CancellationToken ct = default)
        {
            var cart = await _repo.GetCartWithItemsByUserAsync(userId, ct);
            if (cart == null)
                return new CartSummaryDto { CartID = 0, TotalDistinctItems = 0, TotalQuantity = 0 };

            return new CartSummaryDto
            {
                CartID = cart.CartID,
                TotalDistinctItems = cart.Items.Count,
                TotalQuantity = cart.Items.Sum(i => i.Quantity)
            };
        }

        public async Task<CartReadDto?> GetCartByIdAsync(int cartId, CancellationToken ct = default)
        {
            var cart = await _repo.GetCartWithItemsByIdAsync(cartId, ct);
            return cart == null ? null : _mapper.Map<CartReadDto>(cart);
        }

        public async Task<bool> DeleteCartAsync(int cartId, CancellationToken ct = default)
        {
            var cart = await _repo.GetCartWithItemsByIdAsync(cartId, ct);
            if (cart == null) return false;
            _ctx.Carts.Remove(cart);
            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        #region Helpers
        private void Validate(string itemType, int quantity, DateTime? start, DateTime? end)
        {
            if (!ValidItemTypes.Contains(itemType))
                throw new ArgumentException($"Invalid ItemType: {itemType}");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be > 0.");

            var norm = Normalize(itemType);

            if (norm is "Room" or "Vehicle")
            {
                if (start is null || end is null)
                    throw new ArgumentException($"{norm} requires StartDate & EndDate.");
                if (start > end)
                    throw new ArgumentException("StartDate must be <= EndDate.");
            }
            if (norm == "Service")
            {
                if (start != null || end != null)
                    throw new ArgumentException("Service must not have StartDate/EndDate.");
            }
        }

        private static string Normalize(string s) =>
            string.IsNullOrWhiteSpace(s) ? s : char.ToUpperInvariant(s[0]) + s[1..].ToLowerInvariant();
        #endregion
    }
}