using AutoMapper;
using ImageManagement_API.DTOs;
using ImageManagement_API.Models;
using ImageManagement_API.Repositories.Interfaces;
using ImageManagement_API.Services.Interfaces;

namespace ImageManagement_API.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _repository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public ImageService(IImageRepository repository, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _repository = repository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<ImageReadDTO>> GetAllAsync()
        {
            var images = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ImageReadDTO>>(images);
        }

        public async Task<ImageReadDTO?> GetByIdAsync(int id)
        {
            var image = await _repository.GetByIdAsync(id);
            return _mapper.Map<ImageReadDTO?>(image);
        }

        public async Task<ImageReadDTO> CreateAsync(ImageCreateDTO dto)
        {
            var entity = _mapper.Map<Image>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<ImageReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ImageUpdateDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing); // map dto → entity
            _repository.Update(existing);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            _repository.Delete(existing);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<ImageReadDTO?> UploadAndSaveAsync(ImageCreateWithFileDTO dto)
        {
            // Upload ảnh lên Cloudinary
            var uploadResult = await _cloudinaryService.UploadImageAsync(dto.File, "pet_app");
            if (!uploadResult.Success) return null;

            // Lưu metadata vào DB
            var entity = new Image
            {
                IsHotelImg = dto.IsHotelImg,
                IsRoomImg = dto.IsRoomImg,
                IsTourImg = dto.IsTourImg,
                IsVehicleImage = dto.IsVehicleImage,
                LinkedId = dto.LinkedId,
                Caption = dto.Caption,
                IsCover = dto.IsCover,
                ImageUrl = uploadResult.Url ?? "",
                UploadedAt = DateTime.Now
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return _mapper.Map<ImageReadDTO>(entity);
        }
        public async Task<IEnumerable<ImageReadDTO>> UploadAndSaveManyAsync(ImageCreateWithFilesDTO dto)
        {
            var uploadResults = await _cloudinaryService.UploadImagesAsync(dto.Files, "pet_app");
            var entities = new List<Image>();

            foreach (var result in uploadResults)
            {
                if (!result.Success) continue;

                var entity = new Image
                {
                    IsHotelImg = dto.IsHotelImg,
                    IsRoomImg = dto.IsRoomImg,
                    IsTourImg = dto.IsTourImg,
                    IsVehicleImage = dto.IsVehicleImage,
                    LinkedId = dto.LinkedId,
                    Caption = dto.Caption,
                    IsCover = dto.IsCover,
                    ImageUrl = result.Url ?? "",
                    UploadedAt = DateTime.Now
                };
                entities.Add(entity);
                await _repository.AddAsync(entity);
            }

            if (entities.Any())
                await _repository.SaveChangesAsync();

            return _mapper.Map<IEnumerable<ImageReadDTO>>(entities);
        }
        public async Task<IEnumerable<ImageReadDTO>> GetByLinkedIdAsync(Guid linkedId)
        {
            var images = await _repository.GetAllAsync();
            var filtered = images.Where(img => img.LinkedId == linkedId);
            return _mapper.Map<IEnumerable<ImageReadDTO>>(filtered);
        }

    }
}
