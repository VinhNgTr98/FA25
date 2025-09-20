using AutoMapper;
using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Models;

namespace TourAgencyManagement_API.Profiles
{
    public class TourAgencyProfile : Profile
    {
        public TourAgencyProfile()
        {
            // Model -> Read DTO
            CreateMap<TourAgency, TourAgencyReadDto>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserID));

            // Create DTO -> Model
            CreateMap<TourAgencyCreateDto, TourAgency>()
                .ForMember(d => d.UserID, o => o.MapFrom(s => s.UserId));

            // Update DTO -> Model
            CreateMap<TourAgencyUpdateDto, TourAgency>()
                .ForMember(d => d.UserID, o => o.MapFrom(s => s.UserId));
        }
    }
}
