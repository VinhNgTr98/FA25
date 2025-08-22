using AutoMapper;
using OrderManagement_API.DTOs;
using OrderManagement_API.Models;

namespace OrderManagement_API.Profil
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderReadDto>();
            CreateMap<OrderCreateDto, Order>();
            CreateMap<OrderUpdateDto, Order>();
        }
    }
}
