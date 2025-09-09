using AutoMapper;
using VehicleManageMent_API.DTOs;
using VehicleManageMent_API.Models;

namespace VehicleManageMent_API.Profiles
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleReadDTO>();
            CreateMap<VehicleCreateDTO, Vehicle>();
            CreateMap<VehicleUpdateDTO, Vehicle>();
        }
    }
}
