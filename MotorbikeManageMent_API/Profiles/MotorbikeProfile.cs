using AutoMapper;
using MotorbikeManageMent_API.DTOs;
using MotorbikeManageMent_API.Models;


namespace MotorbikeManageMent_API.Profiles
{
    public class MotorbikeProfile : Profile
    {
        public MotorbikeProfile()
        {
            CreateMap<Motorbike, MotorbikeReadDto>();
            CreateMap<MotorbikeCreateDto, Motorbike>();
            CreateMap<MotorbikeUpdateDto, Motorbike>();
        }
    }
}
