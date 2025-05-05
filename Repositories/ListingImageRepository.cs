using Interasian.API.Data;
using Interasian.API.Models;
using Microsoft.EntityFrameworkCore;
using Interasian.API.Utilities;
using Interasian.API.DTOs;

namespace Interasian.API.Repositories
{
	public class ListingImageRepository : IListingImageRepository
	{
		private readonly DatabaseContext _context;

		public ListingImageRepository(DatabaseContext context)
		{
			_context = context;
		}

		public async Task<ListingImage> AddListingImageAsync(ListingImage image)
        {
            await _context.ListingImages.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }
        
        public async Task<PagedList<ListingImage>> GetListingImagesAsync(int listingId, PaginationRequest paginationRequest)
        {
            var query = _context.ListingImages
                .Where(i => i.ListingId == listingId);
                
            return await PagedList<ListingImage>.ToPagedListAsync(
                query,
                paginationRequest.PageNumber,
                paginationRequest.PageSize);
        }
        
        public async Task<ListingImage> GetListingImageByIdAsync(int imageId)
        {
            var image = await _context.ListingImages.FindAsync(imageId);
            return image ?? throw new KeyNotFoundException($"Image with ID {imageId} was not found");
        }
        
        public async Task DeleteListingImageAsync(ListingImage image)
        {
            _context.ListingImages.Remove(image);
            await _context.SaveChangesAsync();
        }
	}
}