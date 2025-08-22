using AutoMapper;
using Categories_API.Models;
using Categories_API.DTOs;

namespace Categories_API.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>().ForMember(dest => dest.CategoryID, opt => opt.Ignore());

        }
    }
}
