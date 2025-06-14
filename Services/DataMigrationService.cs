using Interasian.API.Data;
using Interasian.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Interasian.API.Services
{
    public class DataMigrationService
    {
        private readonly MongoDbContext _mongoContext;

        public DataMigrationService(MongoDbContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task MigrateDataAsync()
        {
            // Check if collections are empty
            var listingsCount = await _mongoContext.Listings.CountDocumentsAsync(FilterDefinition<Listing>.Empty);
            var imagesCount = await _mongoContext.ListingImages.CountDocumentsAsync(FilterDefinition<ListingImage>.Empty);

            if (listingsCount == 0)
            {
                // Seed listings data
                var listings = new List<Listing>
                {
                    new Listing
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Title = "Beachside Villa",
                        Location = "Boracay Island",
                        LandArea = "500 sqm",
                        FloorArea = "350 sqm",
                        BedRooms = 4,
                        BathRooms = 3,
                        Price = 15000000,
                        Description = "A luxurious villa with ocean view and private pool.",
                        Status = true,
                        PropertyType = "House with Lot",
                        Owner = "John Smith",
                        Creator = "jtangpuz@guardian.ph",
                        ImageIds = new List<string>()
                    },
                    new Listing
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Title = "Urban Condo",
                        Location = "Makati City",
                        LandArea = "60 sqm",
                        FloorArea = "60 sqm",
                        BedRooms = 2,
                        BathRooms = 1,
                        Price = 4500000,
                        Description = "Modern condo in the heart of the business district.",
                        Status = true,
                        PropertyType = "Condo",
                        Owner = "Jane Doe",
                        Creator = "jtangpuz@guardian.ph",
                        ImageIds = new List<string>()
                    },
                    new Listing
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Title = "Ayala Building",
                        Location = "Ortigas Center",
                        LandArea = "1200 sqm",
                        FloorArea = "3500 sqm",
                        BedRooms = 8,
                        BathRooms = 8,
                        Price = 75000000,
                        Description = "Prime commercial building with 5 floors suitable for office spaces.",
                        Status = true,
                        PropertyType = "Commercial Building",
                        Owner = "Metro Properties Inc.",
                        Creator = "jtangpuz@guardian.ph",
                        ImageIds = new List<string>()
                    },
                    new Listing
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Title = "Tangpuz House",
                        Location = "Lapu-Lapu City",
                        LandArea = "800 sqm",
                        FloorArea = null,
                        BedRooms = 5,
                        BathRooms = 5,
                        Price = 6500000,
                        Description = "Beautiful vacant lot with panoramic view of Taal Lake.",
                        Status = true,
                        PropertyType = "House with Lot",
                        Owner = "Connie Tangpuz",
                        Creator = "jtangpuz@guardian.ph",
                        ImageIds = new List<string>()
                    },
                    new Listing
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Title = "Suburban Family Home",
                        Location = "Alabang, Muntinlupa",
                        LandArea = "350 sqm",
                        FloorArea = "280 sqm",
                        BedRooms = 5,
                        BathRooms = 4,
                        Price = 18500000,
                        Description = "Spacious family home in a gated community with garden and garage.",
                        Status = true,
                        PropertyType = "House with Lot",
                        Owner = "Maria Santos",
                        Creator = "jtangpuz@guardian.ph",
                        ImageIds = new List<string>()
                    }
                };

                await _mongoContext.Listings.InsertManyAsync(listings);
            }
        }
    }
} 