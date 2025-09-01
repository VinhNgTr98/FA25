using AutoMapper;
using VehicleAgencyManagement_API.DTOs;
using VehicleAgencyManagement_API.Models;

namespace VehicleAgencyManagement_API.Profiles
{
    public class VehicleAgencyProfile : Profile
    {
        public VehicleAgencyProfile()
        {
            CreateMap<VehicleAgency, VehicleAgencyReadDTO>();
            CreateMap<VehicleAgencyCreateDTO, VehicleAgency>();
            CreateMap<VehicleAgencyUpdateDTO, VehicleAgency>();
        }
    }
}
