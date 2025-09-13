using AutoMapper;
using CartManagement_Api.DTOs;
using CartManagement_Api.Models;

namespace CartManagement_Api.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartReadDto>();
            CreateMap<CartCreateDto, Cart>();

            CreateMap<CartItem, CartItemReadDto>();
            CreateMap<CartItemCreateDto, CartItem>();
            CreateMap<CartItemUpdateDto, CartItem>();
        }
    }
}