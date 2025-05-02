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

		public async Task<PagedList<Listing>> GetAllListingsAsync(PaginationRequest paginationRequest, string? searchQuery = null)
		{
			var query = _context.Listings.AsQueryable();

			if (!string.IsNullOrEmpty(searchQuery)) 
			{
				query = query.Where(l => l.Title.Contains(searchQuery) || l.Location.Contains(searchQuery));
			}

			query = query.OrderBy(l => l.ListingId);

			return await PagedList<Listing>.ToPagedListAsync(query, 
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