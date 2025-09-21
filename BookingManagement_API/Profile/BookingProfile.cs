using AutoMapper;
using BookingManagement_API.DTOs;
using BookingManagement_API.Models;
namespace BookingManagement_API.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingReadDto>();

            // Ép Status = Pending khi tạo
            CreateMap<BookingCreateDto, Booking>()
                .ForMember(d => d.Status, opt => opt.MapFrom(_ => "Pending"));

            CreateMap<BookingUpdateDto, Booking>();

            CreateMap<BookingItem, BookingItemReadDto>();
            CreateMap<BookingItemCreateDto, BookingItem>();
            CreateMap<BookingItemUpdateDto, BookingItem>();
        }
    }
}