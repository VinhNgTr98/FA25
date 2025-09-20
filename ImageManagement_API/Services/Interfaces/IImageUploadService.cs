using CloudinaryDotNet.Actions;

namespace ImageManagement_API.Services.Interfaces
{
    public interface IImageUploadService
    {
        Task<ImageUploadResult> UploadAsync(IFormFile file, string folder = "pet_app");
        Task<DeletionResult> DeleteAsync(string publicId);
    }
