using AutoMapper;
using OrderManagement_API.DTOs;
using OrderManagement_API.Models;
namespace OrderManagement_API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderReadDto>();

            // Ép Status = Pending khi tạo
            CreateMap<OrderCreateDto, Order>()
                .ForMember(d => d.Status, opt => opt.MapFrom(_ => "Pending"));

            CreateMap<OrderUpdateDto, Order>();

            CreateMap<OrderItem, OrderItemReadDto>();
            CreateMap<OrderItemCreateDto, OrderItem>();
            CreateMap<OrderItemUpdateDto, OrderItem>();
        }
    }
}