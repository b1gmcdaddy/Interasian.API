using Interasian.API.DTOs;
using Interasian.API.Models;
using Interasian.API.Utilities;

namespace Interasian.API.Repositories
{
	public interface IListingImageRepository
	{
		Task<ListingImage> AddListingImageAsync(ListingImage image);
        Task<PagedList<ListingImage>> GetListingImagesAsync(int listingId, PaginationRequest paginationRequest);
        Task<ListingImage> GetListingImageByIdAsync(int imageId);
        Task DeleteListingImageAsync(ListingImage image);
	}
}