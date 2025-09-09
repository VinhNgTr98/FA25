using System;
using AutoMapper;
using CartManagement_Api.DTOs;
using CartManagement_Api.Models;

namespace CartManagement_Api.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            // Entity -> Read DTO
            CreateMap<Cart, CartReadDto>();
            CreateMap<CartItem, CartItemReadDto>();

            // Create DTO -> Entity
            CreateMap<CartItemCreateDto, CartItem>()
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Update DTO -> Entity (chỉ map StartDate/EndDate nếu != null)
            CreateMap<CartItemUpdateDto, CartItem>()
                .ForMember(d => d.StartDate, opt => opt.Condition(src => src.StartDate.HasValue))
                .ForMember(d => d.EndDate, opt => opt.Condition(src => src.EndDate.HasValue))
                .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}