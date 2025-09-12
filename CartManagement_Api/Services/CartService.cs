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