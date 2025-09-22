using ImageManagement_API.DTOs;
using ImageManagement_API.Services;
using ImageManagement_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImageManagement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageReadDTO>>> GetAll()
        {
            var images = await _imageService.GetAllAsync();
            return Ok(images);
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageReadDTO>> GetById(int id)
        {
            var image = await _imageService.GetByIdAsync(id);
            if (image == null) return NotFound();
            return Ok(image);
        }

        // POST: api/Image
        [HttpPost]
        public async Task<ActionResult<ImageReadDTO>> Create([FromBody] ImageCreateDTO dto)
        {
            var created = await _imageService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ImageID }, created);
        }

        // PUT: api/Image/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ImageUpdateDTO dto)
        {
            var success = await _imageService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/Image/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _imageService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageCreateWithFileDTO dto)
        {
            var result = await _imageService.UploadAndSaveAsync(dto);
            if (result == null)
                return BadRequest(new { success = false, message = "Upload failed" });

            return Ok(result);
        }

        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple([FromForm] ImageCreateWithFilesDTO dto)
        {
            var images = await _imageService.UploadAndSaveManyAsync(dto);

            if (!images.Any())
                return BadRequest("Upload failed for all files.");

            return Ok(images);
        }
        // GET: api/Image/getByLinkedId/{linkedId}
        [HttpGet("getByLinkedId/{linkedId}")]
        public async Task<ActionResult<IEnumerable<ImageReadDTO>>> GetByLinkedId(Guid linkedId)
        {
            var images = await _imageService.GetByLinkedIdAsync(linkedId);
            if (!images.Any())
                return NotFound(new { message = "No images found for this LinkedId" });

            return Ok(images);
        }

        [HttpPut("updateImageUrl/{id}")]
        public async Task<IActionResult> UpdateImageFile(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var updated = await _imageService.UpdateImageFileAsync(id, file);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

    }
}
