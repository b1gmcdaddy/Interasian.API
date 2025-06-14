using MongoDB.Driver;
using Interasian.API.Models;

namespace Interasian.API.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoConnection");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("InterAsianRealty");
        }

        public IMongoCollection<Listing> Listings => _database.GetCollection<Listing>("listings");
        public IMongoCollection<ListingImage> ListingImages => _database.GetCollection<ListingImage>("listingImages");
    }
} 