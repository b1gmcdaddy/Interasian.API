using AutoMapper;
using Interasian.API.DTOs;
using Interasian.API.Models;
using Interasian.API.Repositories;
using Interasian.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Interasian.API.Services;
using CloudinaryDotNet.Actions;

namespace Interasian.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingController : ControllerBase
	{
		private readonly IListingRepository _repo;
		private readonly IUploadService _uploadService;
		private readonly IMapper _mapper;
		readonly ILogger<ListingController> _logger;
		private readonly IWebHostEnvironment _environment;

		public ListingController(
			IListingRepository repo, 
			IUploadService uploadService, 
			IMapper mapper, 
			ILogger<ListingController> logger, 
			IWebHostEnvironment environment)
		{
			_repo = repo;
			_uploadService = uploadService;
			_mapper = mapper;
			_logger = logger;
			_environment = environment;
		}

		[HttpGet]
		public async Task<ActionResult<ApiResponse>> GetAllListings(
			[FromQuery] PaginationRequest paginationRequest, 
			[FromQuery] string? searchQuery = null,
			[FromQuery] string? propertyType = null,
			[FromQuery] SortOptions sortOption = SortOptions.Default
			)
		{
			try
			{
				var listings = await _repo.GetAllListingsAsync(paginationRequest, 
				searchQuery, propertyType, sortOption);
				var listingDtos = _mapper.Map<List<ListingDTO>>(listings);
				
				// Get list of imgs for each listing
				foreach (var listingDto in listingDtos)
				{
					var listing = listings.FirstOrDefault(l => l.ListingId == listingDto.ListingId);
					if (listing?.Images != null)
					{
						listingDto.Images = _mapper.Map<List<ListingImageDTO>>(listing.Images);
					}
				}

				var paginationDetails = PaginationMetadata.FromPagedList(listings);
				return Ok(new ApiResponse(
					true, 
					"Listings retrieved successfully",
					listingDtos,
					paginationDetails));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching listings");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}

		[HttpGet("{listingId}")]
		public async Task<ActionResult<ApiResponse>> GetListingById(int listingId)
		{
			try
			{
				var listing = await _repo.GetListingByIdAsync(listingId);
				if (listing is null)
				{
					return NotFound(new ApiResponse(false, "Listing not found", null!));
				}

				var dto = _mapper.Map<ListingDTO>(listing);
				
				// Include images in the response
				if (listing.Images != null)
				{
					dto.Images = _mapper.Map<List<ListingImageDTO>>(listing.Images);
				}

				return Ok(new ApiResponse(true, "Listing retrieved successfully", dto));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error fetching listing with id: {listingId}");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}

		[HttpPost]
		public async Task<ActionResult<ApiResponse>> CreateListing(CreateListingDTO dto)
		{
			try
			{
				var listing = _mapper.Map<Listing>(dto);
				var created = await _repo.CreateListingAsync(listing);
				var result = _mapper.Map<ListingDTO>(created);

				return CreatedAtAction(
					nameof(GetListingById), 
					new { listingId = result.ListingId }, 
					new ApiResponse(true, "Listing created successfully", result));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "err creating listing");
				return StatusCode(500, new ApiResponse(
					false, "Internal server error", null!));
			}
		}

		[HttpPost("{listingId}/images")]
		public async Task<ActionResult<ApiResponse>> UploadImages(int listingId, IFormFileCollection files)
		{
			try
			{
				var listing = await _repo.GetListingByIdAsync(listingId);
				if (listing is null)
				{
					return NotFound(new ApiResponse(false, "Listing not found", null!));
				}

				var uploadResults = await _uploadService.AddMultiplePhotosAsync(files);
				var images = new List<ListingImage>();

				foreach (var result in uploadResults)
				{
					var image = new ListingImage
					{
						ListingId = listingId,
						FileName = result.SecureUrl.ToString(),
						UploadDate = DateTime.UtcNow
					};
					images.Add(image);
				}

				if (listing.Images == null)
				{
					listing.Images = new List<ListingImage>();
				}
				foreach (var image in images)
				{
					listing.Images.Add(image);
				}

				await _repo.UpdateListingAsync(listing);
				var imageDTOs = _mapper.Map<List<ListingImageDTO>>(images);
				return Ok(new ApiResponse(true, "Images uploaded successfully", imageDTOs));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error uploading images for listing {listingId}");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}

		[HttpDelete("{listingId}/images/{imageId}")]
		public async Task<ActionResult<ApiResponse>> DeleteImage(int listingId, int imageId)
		{
			try
			{
				var listing = await _repo.GetListingByIdAsync(listingId);
				if (listing is null)
				{
					return NotFound(new ApiResponse(false, "Listing not found", null!));
				}

				var image = listing.Images?.FirstOrDefault(i => i.ImageId == imageId);
				if (image is null)
				{
					return NotFound(new ApiResponse(false, "Image not found", null!));
				}

				var publicId = image.FileName.Split('/').Last().Split('.')[0];
				await _uploadService.DeletePhotoAsync(publicId);

				listing.Images?.Remove(image);
				await _repo.UpdateListingAsync(listing);

				return Ok(new ApiResponse(true, "Image deleted successfully", null!));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error deleting image {imageId} from listing {listingId}");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}

		[HttpPut("{listingId}")]
		public async Task<ActionResult<ApiResponse>> UpdateListing(int listingId, CreateListingDTO dto)
		{
			try
			{
				var existing = await _repo.GetListingByIdAsync(listingId);
				if (existing is null)
				{
					return NotFound();
				}
				_mapper.Map(dto, existing);
				await _repo.UpdateListingAsync(existing);

				return Ok(new ApiResponse(true, "Listing updated successfully", null!));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "err updating listing");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}

		[HttpDelete("{listingId}")]
		public async Task<ActionResult<ApiResponse>> Delete(int listingId)
		{
			try
			{
				var existing = await _repo.GetListingByIdAsync(listingId);
				if (existing is null)
				{
					return NotFound();
				}

				await _repo.DeleteListingAsync(existing);
				return Ok(new ApiResponse(true, "Listing deleted successfully", null!));
			}
			catch (Exception ex)
			{
				Log.Error(ex, $"Error deleting listing with ID {listingId}");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}
	}
}
