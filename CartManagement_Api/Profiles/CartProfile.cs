using AutoMapper;
using CartManagement_Api.DTOs;
using CartManagement_Api.Models;

namespace CartManagement_Api.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItem, CartItemReadDto>()
                .ForMember(d => d.RowVersion,
                    o => o.MapFrom(s => s.RowVersion != null ? Convert.ToBase64String(s.RowVersion) : string.Empty));

            CreateMap<Cart, CartReadDto>()
                .ForMember(d => d.RowVersion,
                    o => o.MapFrom(s => s.RowVersion != null ? Convert.ToBase64String(s.RowVersion) : null));

            CreateMap<CartItemCreateDto, CartItem>()
                .ForMember(d => d.RowVersion, o => o.Ignore())
                .ForMember(d => d.CartItemID, o => o.Ignore())
                .ForMember(d => d.Cart, o => o.Ignore());
        }
    }
}