using ImageManagement_API.DTOs;

namespace ImageManagement_API.Services.Interfaces
{
    public interface IImageService
    {
        Task<IEnumerable<ImageReadDTO>> GetAllAsync();
        Task<ImageReadDTO?> GetByIdAsync(int id);
        Task<ImageReadDTO> CreateAsync(ImageCreateDTO dto);
        Task<bool> UpdateAsync(int id, ImageUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<ImageReadDTO?> UploadAndSaveAsync(ImageCreateWithFileDTO dto);
    }
}
