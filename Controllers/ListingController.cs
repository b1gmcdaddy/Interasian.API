using AutoMapper;
using Interasian.API.DTOs;
using Interasian.API.Models;
using Interasian.API.Repositories;
using Interasian.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Interasian.API.Services;
using MongoDB.Driver;
using Interasian.API.Data;
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
		private readonly MongoDbContext _mongoContext;

		public ListingController(
			IListingRepository repo, 
			IUploadService uploadService, 
			IMapper mapper, 
			ILogger<ListingController> logger, 
			IWebHostEnvironment environment,
			MongoDbContext mongoContext)
		{
			_repo = repo;
			_uploadService = uploadService;
			_mapper = mapper;
			_logger = logger;
			_environment = environment;
			_mongoContext = mongoContext;
		}

		#region Get Listings
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
				
				foreach (var listingDto in listingDtos)
				{
					var listing = listings.FirstOrDefault(l => l.Id == listingDto.Id);
					if (listing?.ImageIds != null)
					{
						var images = await _mongoContext.ListingImages
							.Find(x => listing.ImageIds.Contains(x.Id))
							.ToListAsync();
						listingDto.Images = _mapper.Map<List<ListingImageDTO>>(images);
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
		#endregion

		#region Get Listing by Id
		[HttpGet("{listingId}")]
		public async Task<ActionResult<ApiResponse>> GetListingById(string listingId)
		{
			try
			{
				var listing = await _repo.GetListingByIdAsync(listingId);
				if (listing is null)
				{
					return NotFound(new ApiResponse(false, "Listing not found", null!));
				}

				var dto = _mapper.Map<ListingDTO>(listing);
				
				if (listing.ImageIds != null)
				{
					var images = await _mongoContext.ListingImages
						.Find(x => listing.ImageIds.Contains(x.Id))
						.ToListAsync();
					dto.Images = _mapper.Map<List<ListingImageDTO>>(images);
				}

				return Ok(new ApiResponse(true, "Listing retrieved successfully", dto));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error fetching listing with id: {listingId}");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}
		#endregion

		#region Get Count by Property Type
		[HttpGet("count")]
		public async Task<ActionResult<ApiResponse>> GetCountByPropertyType()
		{
			try
			{
				var counts = await _repo.GetCountByPropertyTypeAsync();
				return Ok(new ApiResponse(
					true,
					"Count for property types retrieved successfully",
					counts,
					null));
			} catch (Exception ex)
			{
				_logger.LogError(ex, $"Error fetching counts per property type");
				return StatusCode(500, new ApiResponse(false, "Internal Server Error", null!));
			}
		}
		#endregion

		#region Create Listing
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
					new { listingId = result.Id }, 
					new ApiResponse(true, "Listing created successfully", result));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "err creating listing");
				return StatusCode(500, new ApiResponse(
					false, "Internal server error", null!));
			}
		}
		#endregion

		#region Upload Images
		[HttpPost("{listingId}/images")]
		public async Task<ActionResult<ApiResponse>> UploadImages(string listingId, IFormFileCollection files)
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
					await _mongoContext.ListingImages.InsertOneAsync(image);
					images.Add(image);
				}

				if (listing.ImageIds == null)
				{
					listing.ImageIds = new List<string>();
				}
				listing.ImageIds.AddRange(images.Select(i => i.Id));

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
		#endregion

		#region Delete Image
		[HttpDelete("{listingId}/images/{imageId}")]
		public async Task<ActionResult<ApiResponse>> DeleteImage(string listingId, string imageId)
		{
			try
			{
				var listing = await _repo.GetListingByIdAsync(listingId);
				if (listing is null)
				{
					return NotFound(new ApiResponse(false, "Listing not found", null!));
				}

				var image = await _mongoContext.ListingImages.Find(x => x.Id == imageId).FirstOrDefaultAsync();
				if (image is null)
				{
					return NotFound(new ApiResponse(false, "Image not found", null!));
				}

				var publicId = image.FileName.Split('/').Last().Split('.')[0];
				await _uploadService.DeletePhotoAsync(publicId);

				await _mongoContext.ListingImages.DeleteOneAsync(x => x.Id == imageId);
				listing.ImageIds?.Remove(imageId);
				await _repo.UpdateListingAsync(listing);

				return Ok(new ApiResponse(true, "Image deleted successfully", null!));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error deleting image {imageId} from listing {listingId}");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}
		#endregion

		#region Update Listing
		[HttpPut("{listingId}")]
		public async Task<ActionResult<ApiResponse>> UpdateListing(string listingId, CreateListingDTO dto)
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
		#endregion

		#region Delete Listing
		[HttpDelete("{listingId}")]
		public async Task<ActionResult<ApiResponse>> Delete(string listingId)
		{
			try
			{
				var existing = await _repo.GetListingByIdAsync(listingId);
				if (existing is null)
				{
					return NotFound();
				}

				if (existing.ImageIds != null)
				{
					var images = await _mongoContext.ListingImages
						.Find(x => existing.ImageIds.Contains(x.Id))
						.ToListAsync();

					foreach (var image in images)
					{
						var publicId = image.FileName.Split('/').Last().Split('.')[0];
						await _uploadService.DeletePhotoAsync(publicId);
					}

					await _mongoContext.ListingImages.DeleteManyAsync(x => existing.ImageIds.Contains(x.Id));
				}

				await _repo.DeleteListingAsync(existing);
				return Ok(new ApiResponse(true, "Listing and associated images deleted successfully", null!));
			}
			catch (Exception ex)
			{
				Log.Error(ex, $"Error deleting listing with ID {listingId}");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}
		#endregion
	}
}
