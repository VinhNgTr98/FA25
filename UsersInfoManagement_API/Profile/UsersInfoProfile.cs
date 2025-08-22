using AutoMapper;
using UsersInfoManagement_API.Dtos.UsersInfo;
using UsersInfoManagement_API.Models;

namespace UsersInfoManagement_API.Mapping
{
    public class UsersInfoProfile : Profile
    {
        public UsersInfoProfile()
        {
            CreateMap<UsersInfo, UsersInfoReadDto>();
            CreateMap<UsersInfoCreateDto, UsersInfo>();
            CreateMap<UsersInfoUpdateDto, UsersInfo>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
