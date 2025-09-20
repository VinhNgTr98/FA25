using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImageManagement_API.DTOs;
using ImageManagement_API.Models;
using ImageManagement_API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace ImageManagement_API.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResultDTO> UploadImageAsync(IFormFile file, string folder = "pet_app")
        {
            if (file == null || file.Length == 0)
                return new ImageUploadResultDTO { Success = false, Message = "File is empty" };

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation().Width(800).Height(800).Crop("limit")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                return new ImageUploadResultDTO { Success = false, Message = uploadResult.Error.Message };
            }

            return new ImageUploadResultDTO
            {
                Success = true,
                Message = "Upload successful",
                Url = uploadResult.SecureUrl?.ToString(),
                PublicId = uploadResult.PublicId
            };
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }
    }

}
