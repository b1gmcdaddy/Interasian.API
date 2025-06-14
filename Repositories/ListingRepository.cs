using Interasian.API.Data;
using Interasian.API.Models;
using Interasian.API.Utilities;
using Interasian.API.DTOs;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Interasian.API.Repositories
{
	public class ListingRepository : IListingRepository
	{
		private readonly MongoDbContext _context;

		public ListingRepository(MongoDbContext context)
		{
			_context = context;
		}
		#region GET ALL LISTINGS
		public async Task<PagedList<Listing>> GetAllListingsAsync(
			PaginationRequest paginationRequest, 
			string? searchQuery = null,
			string? propertyType = null,
			SortOptions sortOption = SortOptions.Default
			)
		{
			var filter = Builders<Listing>.Filter.Empty;

			if (!string.IsNullOrEmpty(searchQuery)) 
			{
				var searchFilter = Builders<Listing>.Filter.Or(
					Builders<Listing>.Filter.Regex(x => x.Title, new MongoDB.Bson.BsonRegularExpression(searchQuery, "i")),
					Builders<Listing>.Filter.Regex(x => x.Location, new MongoDB.Bson.BsonRegularExpression(searchQuery, "i"))
				);
				filter &= searchFilter;
			}

			if (!string.IsNullOrEmpty(propertyType))
			{
				filter &= Builders<Listing>.Filter.Eq(x => x.PropertyType, propertyType);
			}

			var query = _context.Listings.Find(filter);
			query = ApplySorting(query, sortOption);

			var totalCount = await query.CountDocumentsAsync();
			var items = await query
				.Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
				.Limit(paginationRequest.PageSize)
				.ToListAsync();

			return new PagedList<Listing>(items, (int)totalCount, paginationRequest.PageNumber, paginationRequest.PageSize);
		}
		#endregion

		#region GET SPECIFIC LISTING
		public async Task<Listing?> GetListingByIdAsync(string listingId)
		{
			return await _context.Listings.Find(x => x.Id == listingId).FirstOrDefaultAsync();
		}
		#endregion
		
		#region GET COUNT
		public async Task<IEnumerable<PropertyTypeCount>> GetCountByPropertyTypeAsync()
		{
			var pipeline = new BsonDocument[]
			{
				new BsonDocument("$group", new BsonDocument
				{
					{ "_id", "$PropertyType" },
					{ "Count", new BsonDocument("$sum", 1) }
				}),
				new BsonDocument("$project", new BsonDocument
				{
					{ "PropertyType", "$_id" },
					{ "Count", 1 },
					{ "_id", 0 }
				})
			};

			var result = await _context.Listings.Aggregate<PropertyTypeCount>(pipeline).ToListAsync();
			return result;
		}
		#endregion
		
		#region CREATE LISTING
		public async Task<Listing> CreateListingAsync(Listing listing)
		{
			await _context.Listings.InsertOneAsync(listing);
			return listing;
		}
		#endregion
		
		#region UPDATE LISTING
		public async Task UpdateListingAsync(Listing listing)
		{
			await _context.Listings.ReplaceOneAsync(x => x.Id == listing.Id, listing);
		}
		#endregion

		#region DELETE LISTING
		public async Task DeleteListingAsync(Listing listing)
		{
			await _context.Listings.DeleteOneAsync(x => x.Id == listing.Id);
		}
		#endregion
		
		#region CHECK IF LISTING EXISTS
		public async Task<bool> ListingExistsAsync(string id) =>
			await _context.Listings.CountDocumentsAsync(x => x.Id == id) > 0;
		#endregion

		#region SORT LISTINGS
		private IFindFluent<Listing, Listing> ApplySorting(IFindFluent<Listing, Listing> query, SortOptions sortOption)
        {
            return sortOption switch
            {
                SortOptions.PriceAsc => query.Sort(Builders<Listing>.Sort.Ascending(x => x.Price)),
                SortOptions.PriceDesc => query.Sort(Builders<Listing>.Sort.Descending(x => x.Price)),
                SortOptions.BedroomsAsc => query.Sort(Builders<Listing>.Sort.Ascending(x => x.BedRooms)),
                SortOptions.BedroomsDesc => query.Sort(Builders<Listing>.Sort.Descending(x => x.BedRooms)),
                SortOptions.BathroomsAsc => query.Sort(Builders<Listing>.Sort.Ascending(x => x.BathRooms)),
                SortOptions.BathroomsDesc => query.Sort(Builders<Listing>.Sort.Descending(x => x.BathRooms)),
                _ => query.Sort(Builders<Listing>.Sort.Ascending(x => x.Id))
            };
        }
		#endregion
	}
}