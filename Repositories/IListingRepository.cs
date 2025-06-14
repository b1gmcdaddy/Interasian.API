using Interasian.API.DTOs;
using Interasian.API.Models;
using Interasian.API.Utilities;

namespace Interasian.API.Repositories
{
	public interface IListingRepository
	{
		Task<PagedList<Listing>> GetAllListingsAsync(
			PaginationRequest paginationRequest, 
			string? searchQuery = null,
			string? propertyType = null,
			SortOptions sortOption = SortOptions.Default
			);
		Task<Listing?> GetListingByIdAsync(string listingId);
		Task<IEnumerable<PropertyTypeCount>> GetCountByPropertyTypeAsync();
		Task<Listing> CreateListingAsync(Listing listing);
		Task UpdateListingAsync(Listing listing);
		Task DeleteListingAsync(Listing listing);
		Task<bool> ListingExistsAsync(string listingId);
	}
}
