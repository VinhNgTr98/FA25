namespace TourManagement.Profiles
{
    using AutoMapper;
    using TourManagement.DTOs;
    using TourManagement.Model;
    public class TourProfile : Profile
    {
        public TourProfile()
        {
            // ----------- TOUR -----------
            CreateMap<TourCreateDTO, Tour>();
            CreateMap<Tour, TourReadDTO>();
            CreateMap<TourUpdateDTO, Tour>();

            // ----------- ITINERARY -----------
            CreateMap<ItineraryCreateDTO, Itinerary>();
            CreateMap<Itinerary, ItineraryReadDTO>();
            CreateMap<ItineraryUpdateDTO, Itinerary>();

            // ----------- TOUR GUIDE -----------
            CreateMap<TourGuideCreateDTO, TourGuide>();
            CreateMap<TourGuide, TourGuideReadDTO>();
            CreateMap<TourGuideUpdateDTO, TourGuide>();

            // ----------- TOUR MEMBER -----------
            CreateMap<TourMemberCreateDTO, TourMember>();
            CreateMap<TourMember, TourMemberReadDTO>();
            CreateMap<TourMemberUpdateDTO, TourMember>();
        }
    }
}
