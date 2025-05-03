using AutoMapper;
using Interasian.API.DTOs;
using Interasian.API.Models;
using Interasian.API.Repositories;
using Interasian.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Interasian.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingImageController : ControllerBase
	{
		private readonly IListingImageRepository _repo;
        private readonly IListingRepository _listingRepo;
		private readonly IMapper _mapper;
		readonly ILogger<ListingController> _logger;
		private readonly IWebHostEnvironment _environment;

		public ListingImageController(IListingImageRepository repo, IListingRepository listingRepo, IMapper mapper, ILogger<ListingController> logger, IWebHostEnvironment environment)
		{
			_repo = repo;
            _listingRepo = listingRepo;
			_mapper = mapper;
			_logger = logger;
			_environment = environment;
		}

		

		[HttpPut("UploadImage")]
		public async Task<ActionResult<ApiResponse>> UploadImage(IFormFile file, int listingId)
		{
			try 
			{
				var listing = await _listingRepo.GetListingByIdAsync(listingId);
				if (listing == null)
				{
					return NotFound(new ApiResponse(false, $"Listing with ID {listingId} not found", null!));
				}
				
				string filepath = GetFilePath(listingId);
				if (!Directory.Exists(filepath))
				{
					Directory.CreateDirectory(filepath);
				}
				
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
				string imagePath = Path.Combine(filepath, fileName);
				
				using (FileStream stream = System.IO.File.Create(imagePath))
				{
					await file.CopyToAsync(stream);
					
					var listingImage = new ListingImage
					{
						ListingId = listingId,
						FileName = fileName,
						UploadDate = DateTime.UtcNow
					};
					
					await _repo.AddListingImageAsync(listingImage);
					
					return Ok(new ApiResponse(true, "Image uploaded successfully", null!));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error uploading image");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}

		[HttpGet("images/{listingId}")]
		public async Task<ActionResult<ApiResponse>> GetListingImages(int listingId)
		{
			try
			{
				var listing = await _listingRepo.GetListingByIdAsync(listingId);
				if (listing == null)
				{
					return NotFound(new ApiResponse(false, $"Listing with ID {listingId} not found", null!));
				}
				
				var images = await _repo.GetListingImagesAsync(listingId);
				var imageDtos = _mapper.Map<List<ListingImageDTO>>(images);
				
				foreach (var imageDto in imageDtos)
				{
					imageDto.ImageUrl = $"{Request.Scheme}://{Request.Host}/api/listing/image/{imageDto.ImageId}";
				}
				
				return Ok(new ApiResponse(true, "Images retrieved successfully", imageDtos));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error retrieving images for listing {listingId}");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}

		[HttpGet("image/{imageId}")]
		public async Task<IActionResult> GetImage(int imageId)
		{
			try
			{
				var image = await _repo.GetListingImageByIdAsync(imageId);
				if (image == null)
				{
					return NotFound();
				}
				
				string imagePath = Path.Combine(GetFilePath(image.ListingId), image.FileName);
				if (!System.IO.File.Exists(imagePath))
				{
					return NotFound();
				}
				
				var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
				return File(imageBytes, GetContentType(Path.GetExtension(image.FileName)));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error retrieving image {imageId}");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpDelete("image/{imageId}")]
		public async Task<IActionResult> DeleteImage(int imageId)
		{
			try
			{
				var image = await _repo.GetListingImageByIdAsync(imageId);
				if (image == null)
				{
					return NotFound();
				}
				
				string imagePath = Path.Combine(GetFilePath(image.ListingId), image.FileName);
				if (System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}
				
				await _repo.DeleteListingImageAsync(image);
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error deleting image {imageId}");
				return StatusCode(500, "Internal server error");
			}
		}

		[NonAction]
		private string GetContentType(string fileExtension)
		{
			switch (fileExtension.ToLower())
			{
				case ".jpg":
				case ".jpeg":
					return "image/jpeg";
				case ".png":
					return "image/png";
				case ".gif":
					return "image/gif";
				case ".bmp":
					return "image/bmp";
				default:
					return "application/octet-stream";
			}
		}

		[NonAction]
		private string GetFilePath(int listingId)
		{
			return _environment.WebRootPath+"\\Uploads\\" + listingId;
		}

		
	}
}
