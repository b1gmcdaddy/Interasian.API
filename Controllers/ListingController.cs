using AutoMapper;
using Interasian.API.DTOs;
using Interasian.API.Models;
using Interasian.API.Repositories;
using Interasian.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace Interasian.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ListingController : ControllerBase
	{
		private readonly IListingRepository _repo;
		private readonly IListingImageRepository _imageRepo;
		private readonly IMapper _mapper;
		readonly ILogger<ListingController> _logger;
		private readonly IWebHostEnvironment _environment;

		public ListingController(
			IListingRepository repo, 
			IListingImageRepository imageRepo, 
			IMapper mapper, 
			ILogger<ListingController> logger, 
			IWebHostEnvironment environment)
		{
			_repo = repo;
			_imageRepo = imageRepo;
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
					var imagePaginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 20 };
					var images = await _imageRepo.GetListingImagesAsync(listingDto.ListingId, imagePaginationRequest);
					var imageDtos = _mapper.Map<List<ListingImageDTO>>(images);
					
					foreach (var imageDto in imageDtos)
					{
						imageDto.ImageUrl = $"{Request.Scheme}://{Request.Host}/api/ListingImage/image/{imageDto.ImageId}";
					}
					listingDto.Images = imageDtos;
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
					return NotFound();
				}

				var dto = _mapper.Map<ListingDTO>(listing);
				
				var imagePaginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 20 };
				var images = await _imageRepo.GetListingImagesAsync(listingId, imagePaginationRequest);
				var imageDtos = _mapper.Map<List<ListingImageDTO>>(images);
				
				foreach (var imageDto in imageDtos)
				{
					imageDto.ImageUrl = $"{Request.Scheme}://{Request.Host}/api/ListingImage/image/{imageDto.ImageId}";
				}
				
				dto.Images = imageDtos;

				return Ok(new ApiResponse(
					true, 
					"Listings retrieved successfully", 
					dto));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error fetching listing with id: {listingId}");
				return StatusCode(500, "internal server err");
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
