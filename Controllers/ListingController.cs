using AutoMapper;
using Interasian.API.DTOs;
using Interasian.API.Models;
using Interasian.API.Repositories;
using Interasian.API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

namespace Interasian.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingController : ControllerBase
	{
		private readonly IListingRepository _repo;
		private readonly IMapper _mapper;
		readonly ILogger<ListingController> _logger;
		private readonly IWebHostEnvironment _environment;

		public ListingController(IListingRepository repo, IMapper mapper, ILogger<ListingController> logger, IWebHostEnvironment environment)
		{
			_repo = repo;
			_mapper = mapper;
			_logger = logger;
			_environment = environment;
		}

		[HttpGet]
		public async Task<ActionResult<ApiResponse>> GetAllListings(
			[FromQuery] PaginationRequest paginationRequest, 
			[FromQuery] string? searchQuery = null,
			[FromQuery] SortOptions sortOption = SortOptions.Default
			)
		{
			try
			{
				var listings = await _repo.GetAllListingsAsync(paginationRequest, searchQuery, sortOption);
				var listingDtos = _mapper.Map<List<ListingDTO>>(listings);
				return Ok(new ApiResponse(true, "Listings retrieved successfully", listingDtos));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching listings");
				return StatusCode(500, new ApiResponse(false, "Internal server error", null!));
			}
		}

		[HttpGet("{listingId}")]
		public async Task<ActionResult<ListingDTO>> GetListingById(int listingId)
		{
			try
			{
				var listing = await _repo.GetListingByIdAsync(listingId);
				if (listing is null)
				{
					return NotFound();
				}

				var dto = _mapper.Map<ListingDTO>(listing);
				return Ok(dto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error fetching listing with id: {listingId}");
				return StatusCode(500, "internal server err");
			}
		}

		[HttpPost]
		public async Task<ActionResult<ListingDTO>> CreateListing(CreateListingDTO dto)
		{
			try
			{
				var listing = _mapper.Map<Listing>(dto);
				var created = await _repo.CreateListingAsync(listing);
				var result = _mapper.Map<ListingDTO>(created);

				return CreatedAtAction(nameof(GetListingById), new { listingId = result.ListingId }, result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "err creating listing");
				return StatusCode(500, "internal server err");
			}
		}


		[HttpPut("{listingId}")]
		public async Task<IActionResult> UpdateListing(int listingId, CreateListingDTO dto)
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

				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "err updating listing");
				return StatusCode(500, "internal server err");
			}
		}

		[HttpDelete("{listingId}")]
		public async Task<IActionResult> Delete(int listingId)
		{
			try
			{
				var existing = await _repo.GetListingByIdAsync(listingId);
				if (existing is null)
				{
					return NotFound();
				}

				await _repo.DeleteListingAsync(existing);
				return NoContent();
			}
			catch (Exception ex)
			{
				Log.Error(ex, $"Error deleting listing with ID {listingId}");
				return StatusCode(500, "Internal server error");
			}
		}
	}
}
