using Interasian.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Interasian.API.Data
{
	public class DatabaseContext : IdentityDbContext<User, IdentityRole, string>
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{
		}

		public DbSet<Listing> Listings { get; set; }
		public DbSet<ListingImage> ListingImages { get; set; }


		// SEEDING DATA
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seed Roles
			modelBuilder.Entity<IdentityRole>().HasData(
				new IdentityRole
				{
					Name = "User",
					NormalizedName = "USER",
					Id = Guid.NewGuid().ToString(),
					ConcurrencyStamp = Guid.NewGuid().ToString()
				},
				new IdentityRole
				{
					Name = "Admin",
					NormalizedName = "ADMIN",
					Id = Guid.NewGuid().ToString(),
					ConcurrencyStamp = Guid.NewGuid().ToString()
				}
			);

			modelBuilder.Entity<ListingImage>()
				.HasOne(i => i.Listing)
				.WithMany(l => l.Images)
				.HasForeignKey(i => i.ListingId)
				.OnDelete(DeleteBehavior.Cascade);

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
					Status = true,
					PropertyType = "House with Lot",
        			Owner = "John Smith"
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
					Status = true,
					PropertyType = "Condo",
        			Owner = "Jane Doe"
				},
				new Listing
				{
					ListingId = 3,
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
					Owner = "Metro Properties Inc."
				},
				new Listing
				{
					ListingId = 4,
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
					Owner = "Connie Tangpuz"
				},
				new Listing
				{
					ListingId = 5,
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
					Owner = "Maria Santos"
				}
			);
		}
	}
}
