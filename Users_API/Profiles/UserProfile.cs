using AutoMapper;
using User_API.DTOs;
using User_API.Models;
using UserManagement_API.DTOs;

namespace User_API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Entity -> ReadDto
            CreateMap<User, UserReadDto>();

            // CreateDto -> Entity (PasswordHash sẽ set trong Service)
            CreateMap<UserCreateDto, User>()
                .ForMember(d => d.PasswordHash, o => o.Ignore());

            // UpdateDto -> Entity
            CreateMap<UserUpdateDto, User>()
                .ForMember(d => d.PasswordHash, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.CountWarning, o => o.Ignore());
        }
    }
}
