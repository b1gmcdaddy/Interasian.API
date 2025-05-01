using Interasian.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Interasian.API.Data
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Listing> Listings { get; set; }



		// SEEDING DATA
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Listing>().HasData(
				new Listing
				{
					ListingId = 1,
					Title = "Beachside Villa",
					Location = "Boracay Island",
					LandArea = "500 sqm",
					FloorArea = "350 sqm",
					BedRooms = 4,
					BathRooms = 3,
					Price = 15000000,
					Description = "A luxurious villa with ocean view and private pool.",
					Status = true
				},
				new Listing
				{
					ListingId = 2,
					Title = "Urban Condo",
					Location = "Makati City",
					LandArea = "60 sqm",
					FloorArea = "60 sqm",
					BedRooms = 2,
					BathRooms = 1,
					Price = 4500000,
					Description = "Modern condo in the heart of the business district.",
					Status = true
				}
			);
		}
	}
}
