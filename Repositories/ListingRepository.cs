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

		public async Task<PagedList<Listing>> GetAllListingsAsync(
			PaginationRequest paginationRequest, 
			string? searchQuery = null,
			SortOptions sortOption = SortOptions.Default
			)
		{
			var query = _context.Listings.AsQueryable();

			if (!string.IsNullOrEmpty(searchQuery)) 
			{
				query = query.Where(l => l.Title.Contains(searchQuery) || l.Location.Contains(searchQuery));
			}

			query = ApplySorting(query, sortOption);

			return await PagedList<Listing>.ToPagedListAsync(
				query, 
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

		private IQueryable<Listing> ApplySorting(IQueryable<Listing> query, SortOptions sortOption)
        {
            return sortOption switch
            {
                SortOptions.PriceAsc => query.OrderBy(l => l.Price),
                SortOptions.PriceDesc => query.OrderByDescending(l => l.Price),
                SortOptions.BedroomsAsc => query.OrderBy(l => l.BedRooms),
                SortOptions.BedroomsDesc => query.OrderByDescending(l => l.BedRooms),
                SortOptions.BathroomsAsc => query.OrderBy(l => l.BathRooms),
                SortOptions.BathroomsDesc => query.OrderByDescending(l => l.BathRooms),
                _ => query.OrderBy(l => l.ListingId) 
            };
        }
	}
}