using AutoMapper;
using ImageManagement_API.DTOs;
using ImageManagement_API.Models;

namespace ImageManagement_API.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            // Entity → DTO (trả dữ liệu ra ngoài)
            CreateMap<Image, ImageReadDTO>();

            // DTO → Entity (khi tạo mới)
            CreateMap<ImageCreateDTO, Image>()
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => DateTime.Now));

            // DTO → Entity (khi cập nhật)
            CreateMap<ImageUpdateDTO, Image>()
                .ForMember(dest => dest.UploadedAt, opt => opt.Ignore()) // không ghi đè UploadedAt
                .ForMember(dest => dest.LinkedId, opt => opt.Ignore()); // không cho đổi liên kết
        }
    }
}
