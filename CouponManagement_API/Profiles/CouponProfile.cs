using AutoMapper;
using CouponManagement_API.DTOs;
using CouponManagement_API.Models;

namespace CouponManagement_API.Profiles
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<Coupon, CouponReadDTO>();
            CreateMap<CouponCreateDTO, Coupon>();
            CreateMap<CouponUpdateDTO, Coupon>();
        }
    }
}
