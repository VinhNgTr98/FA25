using AutoMapper;
using RoleUpdateManagement_API.DTOs;
using RoleUpdateManagement_API.Models;

namespace RoleUpdateManagement_API.Profiles
{
    public class RoleUpdateFormProfile : Profile
    {
        public RoleUpdateFormProfile()
        {
            CreateMap<RoleUpdateForm, RoleUpdateFormReadDTO>();
            CreateMap<RoleUpdateFormCreateDTO, RoleUpdateForm>()
                .ForMember(dest => dest.FormId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.FormStatus, opt => opt.MapFrom(_ => 0)); // default Pending
            CreateMap<RoleUpdateFormUpdateDTO, RoleUpdateForm>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
