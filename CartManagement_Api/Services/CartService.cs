using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CartManagement_Api.Data;
using CartManagement_Api.DTOs;
using CartManagement_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CartManagement_Api.Services
{
    public class CartService : ICartService
    {
        private readonly CartManagement_ApiContext _context;
        private readonly IMapper _mapper;

        public CartService(CartManagement_ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartReadDto> GetCartByUserIdAsync(int userId, CancellationToken ct = default)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserID == userId, ct);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserID = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync(ct);
            }

            // Ensure Items are loaded
            await _context.Entry(cart).Collection(c => c.Items).LoadAsync(ct);

            return _mapper.Map<CartReadDto>(cart);
        }

        public async Task<CartItemReadDto> AddItemAsync(int userId, CartItemCreateDto dto, CancellationToken ct = default)
        {
            // Ensure cart exists
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserID == userId, ct);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserID = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync(ct);
                // Reload with items
                await _context.Entry(cart).Collection(c => c.Items).LoadAsync(ct);
            }

            // Check for existing item (logical key)
            var existing = cart.Items.FirstOrDefault(i =>
                i.ItemType == dto.ItemType &&
                i.ItemID == dto.ItemID &&
                i.StartDate == dto.StartDate &&
                i.EndDate == dto.EndDate
            );

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                existing.UpdatedAt = DateTime.UtcNow;
                cart.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var entity = _mapper.Map<CartItem>(dto);
                entity.CartID = cart.CartID;
                cart.Items.Add(entity);
                cart.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(ct);

            // Return the affected item
            var resultItem = existing ?? cart.Items.Last();
            return _mapper.Map<CartItemReadDto>(resultItem);
        }

        public async Task<CartItemReadDto?> UpdateItemAsync(int cartItemId, CartItemUpdateDto dto, CancellationToken ct = default)
        {
            var item = await _context.CartItems
                .Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.CartItemID == cartItemId, ct);

            if (item == null) return null;

            _mapper.Map(dto, item); // Updates StartDate/EndDate conditionally, sets UpdatedAt
            item.Cart!.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return _mapper.Map<CartItemReadDto>(item);
        }

        public async Task<bool> RemoveItemAsync(int cartItemId, CancellationToken ct = default)
        {
            var item = await _context.CartItems
                .Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.CartItemID == cartItemId, ct);

            if (item == null) return false;

            var cart = item.Cart;
            _context.CartItems.Remove(item);

            if (cart != null)
            {
                cart.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ClearCartAsync(int userId, CancellationToken ct = default)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserID == userId, ct);

            if (cart == null) return false;

            _context.CartItems.RemoveRange(cart.Items);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}