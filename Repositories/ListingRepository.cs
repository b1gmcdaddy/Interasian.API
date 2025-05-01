using Interasian.API.Data;
using Interasian.API.Models;
using Microsoft.EntityFrameworkCore;
using Interasian.API.Utilities;
using Interasian.API.DTOs;

namespace Interasian.API.Repositories
{
	public class ListingRepository : IListingRepository
	{
		private readonly DatabaseContext _context;

		public ListingRepository(DatabaseContext context)
		{
			_context = context;
		}

		public async Task<PagedList<Listing>> GetAllListingsAsync(PaginationRequest paginationRequest)
		{
			return await PagedList<Listing>.ToPagedListAsync(
				_context.Listings.OrderBy(l => l.ListingId), 
				paginationRequest.PageNumber, 
				paginationRequest.PageSize);
		}

		public async Task<Listing?> GetListingByIdAsync(int listingId)
		{
			var listing = await _context.Listings.FindAsync(listingId);
			return listing;
		}

		public async Task<Listing> CreateListingAsync(Listing listing)
		{
			_context.Listings.Add(listing);
			await _context.SaveChangesAsync();
			return listing;
		}

		public async Task UpdateListingAsync(Listing listing)
		{
			_context.Listings.Update(listing);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteListingAsync(Listing listing)
		{
			_context.Listings.Remove(listing);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> ListingExistsAsync(int id) =>
			await _context.Listings.AnyAsync(l => l.ListingId == id);
	}
}


