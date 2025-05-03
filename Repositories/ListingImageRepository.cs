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
        
        public async Task<IEnumerable<ListingImage>> GetListingImagesAsync(int listingId)
        {
            return await _context.ListingImages
                .Where(i => i.ListingId == listingId)
                .OrderByDescending(i => i.UploadDate)
                .ToListAsync();
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