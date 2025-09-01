using AutoMapper;
using TourAgencyManagement_API.DTOs;
using TourAgencyManagement_API.Models;

namespace TourAgencyManagement_API.Profiles
{
    public class TourAgencyProfile : Profile
    {
        public TourAgencyProfile()
        {
            CreateMap<TourAgency, TourAgencyReadDTO>();
            CreateMap<TourAgencyCreateDTO, TourAgency>();
            CreateMap<TourAgencyUpdateDTO, TourAgency>();
        }
    }
}
