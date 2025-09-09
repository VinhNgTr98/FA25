using AutoMapper;
using User_API.DTOs;
using User_API.Models;
using UserManagement_API.DTOs;

namespace UserManagement_API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Entity -> Read DTO (trả đầy đủ theo yêu cầu)
            CreateMap<User, UserReadDto>()
                .ForMember(d => d.Password, opt => opt.MapFrom(s => s.PasswordHash))
                .ForMember(d => d.otp_code, opt => opt.MapFrom(s => s.otp_code))
                .ForMember(d => d.otp_expires, opt => opt.MapFrom(s => s.otp_expires));

            // Create (KHÔNG hash ở đây – hash sẽ làm trong Service)
            CreateMap<UserCreateDto, User>()
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.UsersInfo, opt => opt.Ignore());

            CreateMap<UserWithInfoCreateDto, User>()
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.UsersInfo, opt => opt.MapFrom(s => new UsersInfo
                {
                    FullName = s.FullName,
                    DateOfBirth = s.DateOfBirth,
                    ProfilePictureUrl = s.ProfilePictureUrl,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    Email = s.Email
                }));

            // Update – không đổi password & UsersInfo ở đây (nếu cần update UsersInfo thì tạo DTO riêng)
            CreateMap<UserUpdateDto, User>()
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.UsersInfo, opt => opt.Ignore());
        }
    }
}