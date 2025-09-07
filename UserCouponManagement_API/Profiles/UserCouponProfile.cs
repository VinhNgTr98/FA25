using AutoMapper;
using UserCouponManagement_API.DTOs;
using UserCouponManagement_API.Models;

namespace UserCouponManagement_API.Profiles
{
    public class UserCouponProfile : Profile
    {
        public UserCouponProfile()
        {
            CreateMap<UserCoupon, UserCouponReadDTO>();
            CreateMap<UserCouponCreateDTO, UserCoupon>();
            CreateMap<UserCouponUpdateDTO, UserCoupon>();
        }
    }
}
