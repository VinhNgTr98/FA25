using CloudinaryDotNet.Actions;
using ImageManagement_API.DTOs;

namespace ImageManagement_API.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResultDTO> UploadImageAsync(IFormFile file, string folder = "pet_app");
        Task<bool> DeleteImageAsync(string publicId);

        Task<List<ImageUploadResultDTO>> UploadImagesAsync(IFormFileCollection files, string folder = "pet_app");
    }
}
