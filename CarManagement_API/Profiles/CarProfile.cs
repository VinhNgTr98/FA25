using AutoMapper;
using CarManagement_API.DTOs;
using CarManagement_API.Models;

namespace CarManagement_API.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            // Map từ Car -> CarReadDto
            CreateMap<Car, CarReadDto>();

            // Map từ CarCreateDto -> Car
            CreateMap<CarCreateDto, Car>();

            // Map từ CarUpdateDto -> Car
            CreateMap<CarUpdateDto, Car>();
        }
    }
}
