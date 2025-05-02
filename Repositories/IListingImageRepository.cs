using Interasian.API.DTOs;
using Interasian.API.Models;
using Interasian.API.Utilities;

namespace Interasian.API.Repositories
{
	public interface IListingImageRepository
	{
		Task<ListingImage> AddListingImageAsync(ListingImage image);
        Task<IEnumerable<ListingImage>> GetListingImagesAsync(int listingId);
        Task<ListingImage> GetListingImageByIdAsync(int imageId);
        Task DeleteListingImageAsync(ListingImage image);
	}
}