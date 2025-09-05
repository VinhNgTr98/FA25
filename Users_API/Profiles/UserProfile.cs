using AutoMapper;
using User_API.DTOs;
using User_API.Models;                 // để map User entity
using UserManagement_API.DTOs;         // để map UserCreateDto, UserUpdateDto, v.v.

namespace Users_API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserReadDto>().ReverseMap();
            CreateMap<User, UserWithInfoCreateDto>().ReverseMap(); // nếu muốn map luôn
        }
    }
}
