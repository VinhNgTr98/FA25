using AutoMapper;
using HotelsManagement_API.Models;
using HotelsManagement_API.DTOs;

namespace HotelsManagement_API.Profiles
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<HotelCreateDto, Hotel>();
            CreateMap<Hotel, HotelReadDto>();
            CreateMap<HotelUpdateDto, Hotel>().ForMember(dest => dest.HotelId, opt => opt.Ignore());
        }
    }
}
