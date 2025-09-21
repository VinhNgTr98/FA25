namespace TourManagement.Profiles
{
    using AutoMapper;
    using TourManagement.DTOs;
    using TourManagement.Model;
    public class TourProfile : Profile
    {
        public TourProfile()
        {
            // CreateDTO -> Tour
            CreateMap<TourCreateDTO, Tour>();

            // Tour -> ReadDTO
            CreateMap<Tour, TourReadDTO>();


            // UpdateDTO -> Tour
            CreateMap<TourUpdateDTO, Tour>();

            CreateMap<Itinerary, ItineraryReadDTO>();
            CreateMap<ItineraryCreateDTO, Itinerary>();
            CreateMap<ItineraryUpdateDTO, Itinerary>();
        }
    }
}
