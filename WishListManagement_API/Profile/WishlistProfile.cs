using WishListManagement_API.DTOs;
using WishListManagement_API.Models;

namespace WishListManagement_API.Profile
{
    public class WishlistProfile : AutoMapper.Profile
    {
        public WishlistProfile()
        {
            // Entity -> Response DTO
            CreateMap<Wishlist, WishlistDto>()
                .ForMember(d => d.TargetType, o => o.MapFrom(s =>
                    s.HotelId != null ? WishlistTargetType.Hotel :
                    s.VehiclesId != null ? WishlistTargetType.Vehicle :
                    s.TourId != null ? WishlistTargetType.Tour :
                                           WishlistTargetType.Service))
                .ForMember(d => d.TargetId, o => o.MapFrom(s =>
                    s.HotelId ?? s.VehiclesId ?? s.TourId ?? s.ServiceId ?? Guid.Empty));

            // Create -> Entity
            CreateMap<CreateWishlistDto, Wishlist>()
                .ForMember(d => d.HotelId, o => o.MapFrom(s => s.TargetType == WishlistTargetType.Hotel ? (Guid?)s.TargetId : null))
                .ForMember(d => d.VehiclesId, o => o.MapFrom(s => s.TargetType == WishlistTargetType.Vehicle ? (Guid?)s.TargetId : null))
                .ForMember(d => d.TourId, o => o.MapFrom(s => s.TargetType == WishlistTargetType.Tour ? (Guid?)s.TargetId : null))
                .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.TargetType == WishlistTargetType.Service ? (Guid?)s.TargetId : null));

            // Update -> Entity
            CreateMap<UpdateWishlistDto, Wishlist>()
                .ForMember(d => d.HotelId, o => o.MapFrom(s => s.TargetType == WishlistTargetType.Hotel ? (Guid?)s.TargetId : null))
                .ForMember(d => d.VehiclesId, o => o.MapFrom(s => s.TargetType == WishlistTargetType.Vehicle ? (Guid?)s.TargetId : null))
                .ForMember(d => d.TourId, o => o.MapFrom(s => s.TargetType == WishlistTargetType.Tour ? (Guid?)s.TargetId : null))
                .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.TargetType == WishlistTargetType.Service ? (Guid?)s.TargetId : null));
        }
    }
}
